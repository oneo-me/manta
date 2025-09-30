using System.Text;
using System.Xml;
using Avalonia.Media;
using Avalonia.Platform;
using Axe.Icons.Inners;

namespace Axe.Icons;

public sealed class IconSource
{
    internal double Width { get; }
    internal double Height { get; }

    internal List<IIconLayer> Layers { get; } = [];

    IconSource(string xml)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        var root = xmlDoc.DocumentElement!;

        Width = double.Parse(root.Attributes["width"]!.Value);
        Height = double.Parse(root.Attributes["height"]!.Value);

        foreach (XmlElement node in root.ChildNodes)
            if (node.Name == "path")
            {
                var pathLayer = new IconPathLayer();

                if (node.Attributes["d"]?.Value is { } data)
                    pathLayer.Data = Geometry.Parse(data);
                if (node.Attributes["fill"]?.Value is { } fill)
                    pathLayer.Fill = fill;
                if (node.Attributes["stroke"]?.Value is { } stroke)
                    pathLayer.Stroke = stroke;
                if (node.Attributes["stroke-linecap"]?.Value is { } strokeLineCap)
                    pathLayer.StrokeLineCap = strokeLineCap switch
                    {
                        "round"  => PenLineCap.Round,
                        "square" => PenLineCap.Square,
                        _        => PenLineCap.Flat
                    };
                if (node.Attributes["stroke-width"]?.Value is { } strokeWidthStr && double.TryParse(strokeWidthStr, out var strokeWidth))
                    pathLayer.StrokeWidth = strokeWidth;

                Layers.Add(pathLayer);
            }
    }

    public static IconSource FromAsset(Uri uri, Uri? baseUri = null)
    {
        var stream = AssetLoader.Open(uri, baseUri);
        using var streamReader = new StreamReader(stream);
        var xml = streamReader.ReadToEnd();
        return new(xml);
    }

    public static IconSource FromFile(string path, Encoding? encoding = null)
    {
        var xml = encoding is null ? File.ReadAllText(path) : File.ReadAllText(path, encoding);
        return new(xml);
    }
}
