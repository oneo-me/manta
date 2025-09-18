using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Axe.Themes;

public sealed class AxeVariables : Styles
{
    public AxeVariables(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
