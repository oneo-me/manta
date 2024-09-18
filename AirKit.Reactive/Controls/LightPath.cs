using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace AirKit.Reactive.Controls;

[PseudoClasses(":hide")]
public class LightPath : TemplatedControl
{
    static LightPath()
    {
        AffectsRender<LightPath>(DataProperty);
    }

    public static readonly StyledProperty<Geometry?> DataProperty = AvaloniaProperty.Register<LightPath, Geometry?>(
        nameof(Data));

    public Geometry? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public static readonly StyledProperty<double> SizeProperty = AvaloniaProperty.Register<LightPath, double>(
        nameof(Size), double.NaN);

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DataProperty)
        {
            PseudoClasses.Set(":hide", change.NewValue == null);
        }
        else if (change.Property == SizeProperty && change.NewValue is double size)
        {
            Width = size;
            Height = size;
        }
    }
}
