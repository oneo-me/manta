using AirKit.Reactive.Desktop;
using Avalonia;

namespace AirKit.Reactive.Demo;

static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return DesktopApp.Build<App>();
    }
}
