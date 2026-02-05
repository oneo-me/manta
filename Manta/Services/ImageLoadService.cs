using System.Security.Cryptography;
using System.Text;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Manta.Logging;
using Manta.Media.Imaging;

namespace Manta.Services;

public sealed class ImageLoadService
{
    HttpClient HttpClient { get; } = new();

    public string? CacheDirectory { get; set; }

    const int LockCount = 32;
    readonly SemaphoreSlim[] _locks = CreateLocks(LockCount);

    static SemaphoreSlim[] CreateLocks(int count)
    {
        var locks = new SemaphoreSlim[count];
        for (var i = 0; i < count; i++)
            locks[i] = new SemaphoreSlim(1, 1);
        return locks;
    }

    SemaphoreSlim GetLock(string key) => _locks[(uint)key.GetHashCode() % LockCount];

    internal async Task<Bitmap?> LoadAsync(IImageSource source, CancellationToken cancellationToken)
    {
        if (source is AvaloniaImageSource avaloniaImageSource)
            return await LoadAvaloniaImageAsync(avaloniaImageSource);

        if (source is LocalImageSource localImageSource)
            return await LoadLocalImageAsync(localImageSource, cancellationToken);

        if (source is WebImageSource webImageSource)
            return await LoadWebImageAsync(webImageSource, cancellationToken);

        if (source is BitmapImageSource bitmapImageSource)
            return bitmapImageSource.Bitmap;

        return null;
    }

    string GetCacheFilePath(string source)
    {
        if (string.IsNullOrWhiteSpace(CacheDirectory) || Directory.Exists(CacheDirectory) is false)
            return string.Empty;

        var md5 = GetMd5Hash(source);
        return Path.Combine(CacheDirectory!, $"{md5}.png");
    }

    async Task<Bitmap?> LoadLocalImageAsync(LocalImageSource source, CancellationToken cancellationToken)
    {
        if (File.Exists(source.LocalPath) is false)
            return null;

        var semaphore = GetLock(source.LocalPath);
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            // load from cache first
            var cachedFile = GetCacheFilePath(source.LocalPath);
            if (string.IsNullOrWhiteSpace(cachedFile) is false && File.Exists(cachedFile))
            {
                var cachedBitmap = LoadImageFromCache(cachedFile);
                if (cachedBitmap != null)
                    return cachedBitmap;
            }

            // load from local file
            await using var fileStream = File.OpenRead(source.LocalPath);

            // cache the file
            if (string.IsNullOrWhiteSpace(cachedFile) is false)
            {
                await using var cacheStream = File.Create(cachedFile);
                await fileStream.CopyToAsync(cacheStream, cancellationToken);
                fileStream.Position = 0;
            }

            return new(fileStream);
        }
        finally
        {
            semaphore.Release();
        }
    }

    async Task<Bitmap?> LoadWebImageAsync(WebImageSource source, CancellationToken cancellationToken)
    {
        var uri = source.Uri.ToString();
        var semaphore = GetLock(uri);
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            // load from cache first
            var cachedFile = GetCacheFilePath(uri);
            if (string.IsNullOrWhiteSpace(cachedFile) is false && File.Exists(cachedFile))
            {
                var cachedBitmap = LoadImageFromCache(cachedFile);
                if (cachedBitmap != null)
                    return cachedBitmap;
            }

            // download from web
            var data = await HttpClient.GetByteArrayAsync(source.Uri, cancellationToken);
            if (data.Length == 0)
                return null;

            // cache the file
            if (string.IsNullOrWhiteSpace(cachedFile) is false)
                await File.WriteAllBytesAsync(cachedFile, data, cancellationToken);

            await using var memoryStream = new MemoryStream(data);
            return new(memoryStream);
        }
        finally
        {
            semaphore.Release();
        }
    }

    static Task<Bitmap?> LoadAvaloniaImageAsync(AvaloniaImageSource source)
    {
        using var stream = AssetLoader.Open(source.Uri, source.BaseUri);
        return Task.FromResult<Bitmap?>(new(stream));
    }

    static Bitmap? LoadImageFromCache(string file)
    {
        try
        {
            if (File.Exists(file) is false)
                return null;

            using var fileStream = File.OpenRead(file);
            return new(fileStream);
        }
        catch (Exception e)
        {
            Log.Warning("Failed to load cached image: {0}\n{1}", file, e);

            return null;
        }
    }

    static string GetMd5Hash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);
        return Convert.ToHexString(hashBytes).Replace("-", "");
    }
}
