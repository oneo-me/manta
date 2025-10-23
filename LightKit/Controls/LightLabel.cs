using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class LightLabel : Label
{
    protected override Type StyleKeyOverride { get; } = typeof(LightLabel);

    public static readonly StyledProperty<LightTextStyle> CustomProperty = AvaloniaProperty.Register<LightLabel, LightTextStyle>(
        nameof(Custom));

    public LightTextStyle Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
    }
}
