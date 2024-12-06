using AirKit;
using AirKit.Desktop;
using AirKit.Extensions;
using AirKit.I18N;
using AirKit.Logging;
using AirKit.Paths;
using AirKitDemo.Langs;
using AirKitDemo.Services;
using AirKitDemo.Views;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace AirKitDemo;

sealed class App : LightApp
{
    public App()
    {
        AccentColor = Colors.Teal;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        Lang.Register(LangData.Load<LangKey>());

        var configFile = DesktopApp.Folder.CreateFile("config.yaml");
        var configService = Service.Register(new ConfigService(configFile));

        Lang.Current = configService.Config.Lang;

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
