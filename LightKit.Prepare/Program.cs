using Path = System.IO.Path;

namespace LightKit.Prepare;

static class Program
{
    public static void Main()
    {
        var options = new Options(AppContext.BaseDirectory);

        options.SolutionDir = Path.GetFullPath(Path.Combine(options.AppDir, "../../../.."));
        options.ProjectDir = Path.Combine(options.SolutionDir, "LightKit");
        options.ControlsDir = Path.Combine(options.ProjectDir, "Controls");
        options.DesignControlsDir = Path.Combine(options.ControlsDir, "Design");
        options.BaseControlsDir = Path.Combine(options.ControlsDir, "Base");
        options.SupportedControlsDir = Path.Combine(options.BaseControlsDir, "Supported");
        options.UnSupportedControlsDir = Path.Combine(options.BaseControlsDir, "Unsupported");
        options.BaseThemeFile = Path.Combine(options.ProjectDir, "Themes", "BaseTheme.axaml");

        var scanner = new ControlsScanner(options);
        scanner.Prepare();

        var themeFile = new ThemeFile(options);
        themeFile.Prepare();
    }
}
