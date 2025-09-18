using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Axe.Reactive.Themes;

public sealed class AxeTheme : Styles
{
    public AxeTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
