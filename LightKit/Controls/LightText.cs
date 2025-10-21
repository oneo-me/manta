using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class LightText : TextBlock
{
    protected override Type StyleKeyOverride { get; } = typeof(LightText);

    public static readonly StyledProperty<LightTextStyle> CustomProperty = AvaloniaProperty.Register<LightText, LightTextStyle>(
        nameof(Custom));

    public LightTextStyle Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
    }
}
