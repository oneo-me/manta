using Avalonia;
using Manta;
using Manta.DevTools;

namespace MantaDemo;

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
        var appBuilder = MApp.Build<App>();

#if DEBUG
        appBuilder.UseDevTools();
#endif

        return appBuilder;
    }
}
