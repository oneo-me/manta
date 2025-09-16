using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Axe.Controls.Themes;

public sealed class DefaultTheme : Styles
{
    public DefaultTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
