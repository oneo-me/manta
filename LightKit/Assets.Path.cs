using System.Reflection;
using Avalonia.Media.Imaging;

namespace LightKit;

public static partial class Assets
{
    public static Uri GetUri(string path, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        return new($"avares://{assembly.GetName().Name}/Assets/{path}");
    }

    public static IEnumerable<Uri> GetAssets(string path, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        var uri = GetUri(path, assembly);

        return GetAssets(uri);
    }

    public static bool Exists(string path, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        var uri = GetUri(path, assembly);

        return Exists(uri);
    }

    public static Stream GetStream(string path, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        var uri = GetUri(path, assembly);

        return GetStream(uri);
    }

    public static string GetString(string path, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        var uri = GetUri(path, assembly);

        return GetString(uri);
    }

    public static Bitmap GetBitmap(string path, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        var uri = GetUri(path, assembly);

        return GetBitmap(uri);
    }
}
