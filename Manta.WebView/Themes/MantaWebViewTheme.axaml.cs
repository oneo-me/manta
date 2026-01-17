using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.WebView.Themes;

public sealed class MantaWebViewTheme : Styles
{
    public MantaWebViewTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
