using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace LightKit.Themes;

public sealed class BaseTheme : Styles
{
    public BaseTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
