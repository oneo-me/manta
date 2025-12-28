using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.Themes;

public sealed class BaseTheme : Styles
{
    public BaseTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
