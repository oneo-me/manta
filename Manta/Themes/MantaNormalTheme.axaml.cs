using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.Themes;

public sealed class MantaNormalTheme : Styles
{
    public MantaNormalTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
