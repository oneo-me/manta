using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.Themes;

public sealed class MantaTheme : Styles
{
    public MantaTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
