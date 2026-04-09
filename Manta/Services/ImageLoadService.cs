using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Manta.Extensions;
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
            locks[i] = new(1, 1);
        return locks;
    }

    SemaphoreSlim GetLock(string key)
    {
        return _locks[(uint)key.GetHashCode() % LockCount];
    }

    internal async Task<Bitmap?> LoadAsync(IImageSource source, CancellationToken cancellationToken)
    {
        if (source is AvaloniaImageSource avaloniaImageSource)
            return await LoadAvaloniaImageAsync(avaloniaImageSource).ConfigureAwait(false);

        if (source is LocalImageSource localImageSource)
            return await LoadLocalImageAsync(localImageSource, cancellationToken).ConfigureAwait(false);

        if (source is WebImageSource webImageSource)
            return await LoadWebImageAsync(webImageSource, cancellationToken).ConfigureAwait(false);

        if (source is BitmapImageSource bitmapImageSource)
            return bitmapImageSource.Bitmap;

        return null;
    }

    string GetCacheFilePath(string source)
    {
        if (string.IsNullOrWhiteSpace(CacheDirectory) || Directory.Exists(CacheDirectory) is false)
            return string.Empty;

        var md5 = source.GetHashHex();
        return Path.Combine(CacheDirectory!, $"{md5}.png");
    }

    async Task<Bitmap?> LoadLocalImageAsync(LocalImageSource source, CancellationToken cancellationToken)
    {
        if (File.Exists(source.LocalPath) is false)
            return null;

        var semaphore = GetLock(source.LocalPath);
        await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
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
            var fileStream = File.OpenRead(source.LocalPath);
            try
            {
                // cache the file
                if (string.IsNullOrWhiteSpace(cachedFile) is false)
                {
                    var cacheStream = File.Create(cachedFile);
                    await fileStream.CopyToAsync(cacheStream, cancellationToken).ConfigureAwait(false);
                    await cacheStream.DisposeAsync().ConfigureAwait(false);
                    fileStream.Position = 0;
                }

                return new(fileStream);
            }
            finally
            {
                await fileStream.DisposeAsync().ConfigureAwait(false);
            }
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
        await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
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
            var data = await HttpClient.GetByteArrayAsync(source.Uri, cancellationToken).ConfigureAwait(false);
            if (data.Length == 0)
                return null;

            // cache the file
            if (string.IsNullOrWhiteSpace(cachedFile) is false)
                await File.WriteAllBytesAsync(cachedFile, data, cancellationToken).ConfigureAwait(false);

            var memoryStream = new MemoryStream(data);
            try
            {
                return new(memoryStream);
            }
            finally
            {
                await memoryStream.DisposeAsync().ConfigureAwait(false);
            }
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
}
