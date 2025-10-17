using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace LightKit.Wind;

public sealed class WindTheme : Styles
{
    public WindTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
