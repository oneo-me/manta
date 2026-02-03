using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.NodeEditor.Themes;

public sealed class MantaNodeEditorTheme : Styles
{
    public MantaNodeEditorTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
