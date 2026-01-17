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
        return MApp.Build<App>()
            .UseDevTools(new()
            {
                AutoShowByDefault = true
            });
    }
}
