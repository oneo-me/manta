using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace AirKit.Reactive.Themes;

public sealed class LightThemeBase : Styles
{
    public LightThemeBase(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
