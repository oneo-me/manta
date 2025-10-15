using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace LightKit.Themes;

public sealed class LightKitTheme : Styles
{
    public LightKitTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
