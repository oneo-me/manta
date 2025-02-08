using System.Collections.ObjectModel;
using AirKit.Extensions;
using AirKit.Logging;
using AirKit.Paths;
using Avalonia;

namespace AirKit;

public static class DesktopApp
{
    public static AppBase Current => Application.Current as AppBase ?? throw new InvalidOperationException("Application is not AppBase");

    public static FileInfo File { get; }
    public static DirectoryInfo Folder { get; }
    public static ReadOnlyCollection<string> CommandLineArgs { get; }

    static DesktopApp()
    {
        var args = Environment.GetCommandLineArgs();
        File = new(args[0]);

        if (string.Equals(File.GetFileNameWithoutExtension(), "Avalonia.Designer.HostApp", StringComparison.Ordinal))
        {
            CommandLineArgs = new([]);
            File = new(args[^1]);
        }
        else
        {
            CommandLineArgs = new(args[1..]);
        }

        Folder = File.Directory!;
    }

    public static void Main(Func<AppBuilder> buildAvaloniaApp, string[] args)
    {
        try
        {
            Log.Information("App", "Application is starting up");

            buildAvaloniaApp()
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

    public static AppBuilder Build<T>() where T : AppBase, new()
    {
        return AppBuilder.Configure<T>()
            .UseDefaultFontFamily()
            .UsePlatformDetect()
            .UseAvaloniaLogger();
    }
}
