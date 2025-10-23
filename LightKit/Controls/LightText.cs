using Avalonia.Controls;

namespace LightKit.Controls;

public class LightText : TextBlock
{
    protected override Type StyleKeyOverride { get; } = typeof(LightText);
}
