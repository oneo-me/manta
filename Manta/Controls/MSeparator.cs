using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace Manta.Controls;

public class MSeparator : Separator
{
    public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<MSeparator, Orientation>(
        nameof(Orientation));

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }
}
