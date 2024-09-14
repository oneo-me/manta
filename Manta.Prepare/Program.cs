using Path = System.IO.Path;

namespace Manta.Prepare;

static class Program
{
    public static void Main()
    {
        try
        {
            var options = new Options(AppContext.BaseDirectory);

            Console.WriteLine("Preparing Manta...");

            options.SolutionDir = Path.GetFullPath(Path.Combine(options.AppDir, "../../../.."));
            options.ProjectDir = Path.Combine(options.SolutionDir, "Manta");
            options.ControlsDir = Path.Combine(options.ProjectDir, "Controls");
            options.DesignControlsDir = Path.Combine(options.ControlsDir, "Design");
            options.BaseControlsDir = Path.Combine(options.ControlsDir, "Base");
            options.SupportedControlsDir = Path.Combine(options.BaseControlsDir, "Supported");
            options.UnSupportedControlsDir = Path.Combine(options.BaseControlsDir, "Unsupported");
            options.BaseThemeFile = Path.Combine(options.ProjectDir, "Themes", "BaseTheme.axaml");

            Console.WriteLine("Scanning controls...");

            var scanner = new ControlsScanner(options);
            scanner.Prepare();

            Console.WriteLine("Preparing theme file...");

            var themeFile = new ThemeFile(options);
            themeFile.Prepare();
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred during Manta preparation:");
            Console.WriteLine(e);
        }
        finally
        {
            Console.WriteLine("Manta preparation completed.");
        }
    }
}
