using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.Markdown.Themes;

public sealed class MantaMarkdownTheme : Styles
{
    public MantaMarkdownTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
