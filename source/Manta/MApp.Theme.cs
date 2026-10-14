using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Styling;

namespace Manta;

public partial class MApp
{
    public static ThemeVariant SwitchTheme()
    {
        if (Application.Current is not { } app)
            return ThemeVariant.Default;

        if (app.RequestedThemeVariant == ThemeVariant.Default)
            app.RequestedThemeVariant = app.ActualThemeVariant == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;
        else
            app.RequestedThemeVariant = ThemeVariant.Default;

        return app.RequestedThemeVariant;
    }

    public static IDisposable SubscribeThemeChanged(Action<ThemeVariant?> onThemeChanged)
    {
        if (Application.Current is not { } app)
            return Disposable.Empty;

        return app.GetObservable(RequestedThemeVariantProperty)
            .Subscribe(onThemeChanged);
    }
}
