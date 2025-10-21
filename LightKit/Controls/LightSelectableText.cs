using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class LightSelectableText : SelectableTextBlock
{
    protected override Type StyleKeyOverride { get; } = typeof(LightSelectableText);

    public static readonly StyledProperty<LightTextStyle> CustomProperty = AvaloniaProperty.Register<LightSelectableText, LightTextStyle>(
        nameof(Custom));

    public LightTextStyle Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
    }
}
