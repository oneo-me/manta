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

    internal Task<Bitmap?> LoadAsync(IImageSource source, CancellationToken cancellationToken)
    {
        if (source is AvaloniaImageSource avaloniaImageSource)
            return LoadAvaloniaImageAsync(avaloniaImageSource);

        if (source is LocalImageSource localImageSource)
            return LoadLocalImageAsync(localImageSource, cancellationToken);

        if (source is WebImageSource webImageSource)
            return LoadWebImageAsync(webImageSource, cancellationToken);

        return Task.FromResult<Bitmap?>(null);
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

        // TODO A lock should be created to prevent multiple threads from processing the same raw file simultaneously.

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

    async Task<Bitmap?> LoadWebImageAsync(WebImageSource source, CancellationToken cancellationToken)
    {
        // TODO A lock should be created to prevent multiple threads from processing the same raw file simultaneously.

        // load from cache first
        var cachedFile = GetCacheFilePath(source.Uri.ToString());
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
