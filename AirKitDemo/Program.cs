using AirKit;
using Avalonia;

namespace AirKitDemo;

static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        DesktopApp.Main(BuildAvaloniaApp, args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return DesktopApp.Build<App>();
    }
}
