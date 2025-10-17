using Avalonia.Controls;

namespace LightKit.Controls;

sealed class LightWindow : Window
{
    protected override Type StyleKeyOverride { get; } = typeof(LightWindow);
}
