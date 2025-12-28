using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Manta.Controls;

public class MtPathIcon : TemplatedControl
{
    static MtPathIcon()
    {
        AffectsRender<MtPathIcon>(DataProperty);
    }

    public static readonly StyledProperty<Geometry?> DataProperty = AvaloniaProperty.Register<MtPathIcon, Geometry?>(
        nameof(Data));

    public Geometry? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public static readonly StyledProperty<double> SizeProperty = AvaloniaProperty.Register<MtPathIcon, double>(
        nameof(Size));

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public static readonly StyledProperty<bool> UseStrokeProperty = AvaloniaProperty.Register<MtPathIcon, bool>(
        nameof(UseStroke));

    public bool UseStroke
    {
        get => GetValue(UseStrokeProperty);
        set => SetValue(UseStrokeProperty, value);
    }

    public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<MtPathIcon, double>(
        nameof(StrokeThickness));

    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly StyledProperty<PenLineCap> StrokeLineCapProperty = AvaloniaProperty.Register<MtPathIcon, PenLineCap>(
        nameof(StrokeLineCap));

    public PenLineCap StrokeLineCap
    {
        get => GetValue(StrokeLineCapProperty);
        set => SetValue(StrokeLineCapProperty, value);
    }
}
