using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class LightLabel : Label
{
    protected override Type StyleKeyOverride { get; } = typeof(LightLabel);

    public static readonly StyledProperty<LightTextSize> CustomProperty = AvaloniaProperty.Register<LightLabel, LightTextSize>(
        nameof(Custom));

    public LightTextSize Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
    }
}
