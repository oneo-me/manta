using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class LightButton : Button
{
    protected override Type StyleKeyOverride { get; } = typeof(LightButton);

    public static readonly StyledProperty<LightButtonStyle> CustomProperty = AvaloniaProperty.Register<LightButton, LightButtonStyle>(
        nameof(Custom));

    public LightButtonStyle Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
    }

    public static readonly StyledProperty<LightButtonSize> SizeProperty = AvaloniaProperty.Register<LightButton, LightButtonSize>(
        nameof(Size));

    public LightButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
