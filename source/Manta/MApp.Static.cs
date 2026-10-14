using System.Collections.ObjectModel;

namespace Manta;

public partial class MApp
{
    public static string File { get; }
    public static string Folder { get; }
    public static ReadOnlyCollection<string> CommandLineArgs { get; }

    static MApp()
    {
        var args = Environment.GetCommandLineArgs();
        File = args[0];

        if (string.Equals(Path.GetFileNameWithoutExtension(File), "Avalonia.Designer.HostApp", StringComparison.Ordinal))
        {
            CommandLineArgs = new([]);
            File = args[^1];
        }
        else
        {
            CommandLineArgs = new(args[1..]);
        }

        Folder = Path.GetDirectoryName(File)!;
    }
}
