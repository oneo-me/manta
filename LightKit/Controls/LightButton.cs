using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace LightKit.Controls;

public class LightButton : Button
{
    public static readonly StyledProperty<LightButtonType> TypeProperty = AvaloniaProperty.Register<LightButton, LightButtonType>(
        nameof(Type));

    public LightButtonType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public static readonly StyledProperty<LightButtonSize> SizeProperty = AvaloniaProperty.Register<LightButton, LightButtonSize>(
        nameof(Size));

    public LightButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<LightButton, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }
}
