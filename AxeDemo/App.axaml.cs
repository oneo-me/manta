using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Axe.I18N;
using Axe.I18N.Avalonia;
using Axe.Logging;
using Axe.Reactive;
using Axe.Reactive.Services;
using Axe.Reactive.Views;
using AxeDemo.Langs;
using AxeDemo.Services;
using AxeDemo.Views;

namespace AxeDemo;

sealed class App : AxeApp
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnInitialize(out WindowModel? mainModel, out Action? onExit)
    {
        var appFolder = DesktopApp.Folder;
        var dataFolder = Path.Combine(appFolder, "Data");
        Directory.CreateDirectory(dataFolder);

        if (Design.IsDesignMode)
            Log.AddLogger(new ConsoleLogger(LogTimeType.None, LogLevel.All));
        else if (Debugger.IsAttached)
            Log.AddLogger(new ConsoleLogger(LogTimeType.Time, LogLevel.All));
        else
            Log.AddLogger(new ConsoleLogger(LogTimeType.DateTime, LogLevel.All));

        Lang.Register(LangData.LoadFromAssembly<LangKey>());

        var configService = Service.Register(new ConfigService(Path.Combine(dataFolder, "config.json")));
        var appService = Service.Register(new ViewService(configService));
        Service.Register(new DemoService());

        Lang.Current = configService.Config.Language;

        mainModel = new Main_Model();
        onExit = () =>
        {
            appService.Save();
            configService.Save();
        };
    }
}
