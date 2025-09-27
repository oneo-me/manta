using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Axe.I18N;
using Axe.I18N.Avalonia;
using Axe.Logging;
using Axe.Reactive;
using AxeDemo.Langs;
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
        if (Design.IsDesignMode)
            Log.AddLogger(new ConsoleLogger(LogTimeType.None, LogLevel.All));
        else if (Debugger.IsAttached)
            Log.AddLogger(new ConsoleLogger(LogTimeType.Time, LogLevel.All));
        else
            Log.AddLogger(new ConsoleLogger(LogTimeType.DateTime, LogLevel.All));

        Lang.Register(LangData.LoadFromAssembly<LangKey>());

        mainModel = new Main_Model();
        onExit = null;
    }
}
