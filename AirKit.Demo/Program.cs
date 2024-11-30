using AirKit.Desktop;
using AirKit.Logging;
using Avalonia;

namespace AirKit.Demo;

static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("App", "Application is starting up");

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            Log.Fatal("App", "An unexpected error occurred: {Error}", e);
            throw;
        }
        finally
        {
            Log.Information("App", "Application is shutting down");
        }
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return DesktopApp.Build<App>();
    }
}
