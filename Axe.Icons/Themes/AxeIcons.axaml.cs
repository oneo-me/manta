using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Axe.Icons.Themes;

public sealed class AxeIcons : Styles
{
    public AxeIcons(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
