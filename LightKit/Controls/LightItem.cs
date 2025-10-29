using Avalonia.Controls;

namespace LightKit.Controls;

public class LightItem : ContentControl
{
    protected override Type StyleKeyOverride { get; } = typeof(LightItem);
}
