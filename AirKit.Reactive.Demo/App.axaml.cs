using AirKit.Reactive.Demo.Views;
using AirKit.Reactive.Extensions;
using AirKit.Reactive.Logging;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace AirKit.Reactive.Demo;

sealed class App : LightApp
{
    public App()
    {
        Name = $"{nameof(AirKit)}.{nameof(Reactive)}.{nameof(Demo)}";
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        if (Design.IsDesignMode)
        {
            Log.Debug("App", "Design mode is enabled");
            return;
        }

        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        desktop.MainWindow = new Main_Model().CreateWindow();
    }
}
