using Avalonia.Controls;

namespace Axe.Reactive.Controls;

sealed class ReactiveWindow : Window
{
    protected override Type StyleKeyOverride { get; } = typeof(ReactiveWindow);
}
