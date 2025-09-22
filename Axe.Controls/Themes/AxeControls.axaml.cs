using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Axe.Controls.Themes;

public sealed class AxeControls : Styles
{
    public AxeControls(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
