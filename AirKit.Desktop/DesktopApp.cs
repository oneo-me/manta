using System.Collections.ObjectModel;
using AirKit.Paths;

namespace AirKit.Desktop;

public static partial class DesktopApp
{
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
}
