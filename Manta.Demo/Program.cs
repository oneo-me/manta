using System.Diagnostics;
using Avalonia;
using Manta.DevTools;

namespace Manta.Demo;

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
        appBuilder.UseDevTools(new()
        {
            AutoShowByDefault = Debugger.IsAttached
        });
#endif

        return appBuilder;
    }
}
