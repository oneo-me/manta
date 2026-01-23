using Avalonia.Media;

namespace Manta.Controls.Icons.Inners;

sealed class IconPathLayer : IIconLayer
{
    public string Data { get; set; } = string.Empty;
    public string Fill { get; set; } = string.Empty;
    public string Stroke { get; set; } = string.Empty;
    public PenLineCap StrokeLineCap { get; set; } = PenLineCap.Flat;
    public double StrokeWidth { get; set; } = 1d;
}
