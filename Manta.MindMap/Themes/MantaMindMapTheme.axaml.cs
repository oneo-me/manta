using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.MindMap.Themes;

public sealed class MantaMindMapTheme : Styles
{
    public MantaMindMapTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
