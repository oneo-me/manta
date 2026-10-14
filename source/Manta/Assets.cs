using System.Reflection;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Manta.Controls.Icons;

namespace Manta;

public sealed class Assets(Assembly assembly)
{
    readonly string _assemblyName = assembly.GetName().Name ?? throw new InvalidOperationException("Assembly name cannot be null.");

    Uri GetUri(string path)
    {
        return new($"avares://{_assemblyName}/Assets/{path}");
    }

    public IEnumerable<string> GetAssets(string path)
    {
        var uri = GetUri(path);

        foreach (var asset in AssetLoader.GetAssets(uri, null))
            yield return asset.ToString();
    }

    public bool Exists(string path)
    {
        var uri = GetUri(path);
        return AssetLoader.Exists(uri);
    }

    public Stream GetStream(string path)
    {
        var uri = GetUri(path);
        var stream = AssetLoader.Open(uri);
        return stream ?? throw new InvalidOperationException($"Asset '{uri}' not found.");
    }

    public string GetString(string path)
    {
        using var stream = GetStream(path);
        using var streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
    }

    public Bitmap GetBitmap(string path)
    {
        using var stream = GetStream(path);
        return new(stream);
    }

    public byte[] GetBytes(string path)
    {
        using var stream = GetStream(path);
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    public IconSource GetIcon(string path)
    {
        using var stream = GetStream(path);
        using var streamReader = new StreamReader(stream);
        var xml = streamReader.ReadToEnd();
        return new(xml);
    }
}
