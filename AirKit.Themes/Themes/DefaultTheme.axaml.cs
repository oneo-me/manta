using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace AirKit.Themes.Themes;

public class DefaultTheme : Styles
{
    public DefaultTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
