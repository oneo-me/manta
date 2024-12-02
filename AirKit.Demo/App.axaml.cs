using AirKit.Demo.Services;
using AirKit.Demo.Views;
using AirKit.Desktop;
using AirKit.Extensions;
using AirKit.Logging;
using AirKit.Paths;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace AirKit.Demo;

sealed class App : LightApp
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        var configFile = DesktopApp.Folder.CreateFile("config.yaml");
        var configService = Service.Register(new ConfigService(configFile));

        if (Design.IsDesignMode)
        {
            Log.Information("App", "Design mode is enabled");
            return;
        }

        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        desktop.MainWindow = new Main_Model().CreateWindow();

        desktop.Exit += delegate
        {
            configService.Save();
        };
    }
}
