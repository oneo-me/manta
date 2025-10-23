using Avalonia.Controls;

namespace LightKit.Controls;

public class LightSelectableText : SelectableTextBlock
{
    protected override Type StyleKeyOverride { get; } = typeof(LightSelectableText);
}
