using Avalonia.Controls;

namespace LightKit.Controls;

public class LightLabel : Label
{
    protected override Type StyleKeyOverride { get; } = typeof(LightLabel);
}
