using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Manta.Controls;

public class MNumericUpDown : NumericUpDown
{
    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<MNumericUpDown, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }
}
