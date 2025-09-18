using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Axe.Logging;
using Axe.Reactive;
using ReactiveDemo.Services;
using ReactiveDemo.Views;

namespace ReactiveDemo;

sealed class App : AxeApp
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnInitialize(out WindowModel? mainModel, out Action? onExit)
    {
        if (Design.IsDesignMode)
            Log.AddLogger(new ConsoleLogger(LogTimeType.None, LogLevel.All));
        else if (Debugger.IsAttached)
            Log.AddLogger(new ConsoleLogger(LogTimeType.Time, LogLevel.All));
        else
            Log.AddLogger(new ConsoleLogger(LogTimeType.DateTime, LogLevel.All));

        Service.Register(new AppService());

        mainModel = new Main_Model();
        onExit = null;

        Log.Information("Application initialized");
        Log.Verbose("Test verbose log");
        Log.Debug("Test debug log");
        Log.Warning("Test warning log");
        Log.Error("Test error log");
        Log.Fatal("Test important log");
    }
}
