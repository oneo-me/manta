using Avalonia.Controls;

namespace LightKit.Controls;

sealed class ReactiveWindow : Window
{
    protected override Type StyleKeyOverride { get; } = typeof(ReactiveWindow);
}
