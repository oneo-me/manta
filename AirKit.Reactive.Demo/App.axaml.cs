using AirKit.Reactive.Demo.Views;
using AirKit.Reactive.Extensions;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace AirKit.Reactive.Demo;

public class App : LightApp
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        if (Design.IsDesignMode)
            return;

        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        desktop.MainWindow = new Main_Model().CreateWindow();
    }
}
