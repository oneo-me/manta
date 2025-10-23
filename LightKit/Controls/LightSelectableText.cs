using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class LightSelectableText : SelectableTextBlock
{
    protected override Type StyleKeyOverride { get; } = typeof(LightSelectableText);

    public static readonly StyledProperty<LightTextSize> CustomProperty = AvaloniaProperty.Register<LightSelectableText, LightTextSize>(
        nameof(Custom));

    public LightTextSize Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
    }
}
