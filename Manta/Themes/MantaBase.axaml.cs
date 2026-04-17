using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.Themes;

public sealed class MantaBase : Styles
{
    public MantaBase(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
