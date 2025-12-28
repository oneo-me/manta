using Avalonia.Media;

namespace Manta.Controls.Icons.Inners;

sealed class IconPathLayer : IIconLayer
{
    static readonly Geometry _emptyGeometry = Geometry.Parse(string.Empty);

    public Geometry Data { get; set; } = _emptyGeometry;
    public string Fill { get; set; } = string.Empty;
    public string Stroke { get; set; } = string.Empty;
    public PenLineCap StrokeLineCap { get; set; } = PenLineCap.Flat;
    public double StrokeWidth { get; set; } = 1d;
}
