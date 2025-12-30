using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.Charts.Themes;

public sealed class MantaChartsTheme : Styles
{
    public MantaChartsTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
