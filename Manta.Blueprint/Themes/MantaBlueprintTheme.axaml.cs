using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.Blueprint.Themes;

public sealed class MantaBlueprintTheme : Styles
{
    public MantaBlueprintTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
