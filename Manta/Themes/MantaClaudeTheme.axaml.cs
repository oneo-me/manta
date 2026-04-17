using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.Themes;

public sealed class MantaClaudeTheme : Styles
{
    public MantaClaudeTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
