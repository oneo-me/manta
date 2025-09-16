using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Axe.Reactive.Themes;

public sealed class OrphicTheme : Styles
{
    public OrphicTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
