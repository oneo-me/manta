using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Manta;

public static partial class Assets
{
    public static IEnumerable<Uri> GetAssets(Uri uri)
    {
        foreach (var asset in AssetLoader.GetAssets(uri, null))
            yield return asset;
    }

    public static bool Exists(Uri uri)
    {
        return AssetLoader.Exists(uri);
    }

    public static Stream GetStream(Uri uri)
    {
        var stream = AssetLoader.Open(uri);
        return stream ?? throw new InvalidOperationException($"Asset '{uri}' not found.");
    }

    public static string GetString(Uri uri)
    {
        using var stream = GetStream(uri);
        using var streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
    }

    public static Bitmap GetBitmap(Uri uri)
    {
        using var stream = GetStream(uri);
        return new(stream);
    }

    public static byte[] GetBytes(Uri uri)
    {
        using var stream = GetStream(uri);
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
