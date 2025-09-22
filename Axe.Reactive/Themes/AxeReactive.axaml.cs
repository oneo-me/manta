using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Axe.Reactive.Themes;

public sealed class AxeReactive : Styles
{
    public AxeReactive(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
