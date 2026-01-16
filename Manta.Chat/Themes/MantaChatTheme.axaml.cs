using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Manta.Chat.Themes;

public sealed class MantaChatTheme : Styles
{
    public MantaChatTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
