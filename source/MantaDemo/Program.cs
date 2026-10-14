using Avalonia;
using Manta;

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
        return MApp.Build<App>();
    }
}
