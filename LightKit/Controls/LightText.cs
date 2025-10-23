using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class LightText : TextBlock
{
    protected override Type StyleKeyOverride { get; } = typeof(LightText);

    public static readonly StyledProperty<LightTextSize> SizeProperty = AvaloniaProperty.Register<LightText, LightTextSize>(
        nameof(Size));

    public LightTextSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
