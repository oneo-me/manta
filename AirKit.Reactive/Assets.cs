using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace AirKit.Reactive;

public static partial class Assets
{
    public static IEnumerable<Uri> GetAssets(Uri uri)
    {
        return AssetLoader.GetAssets(uri, null);
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
}
