namespace LightKit.Prepare;

static class Program
{
    public static void Main()
    {
        var appDir = AppContext.BaseDirectory;
        var sourceDir = Path.GetFullPath(Path.Combine(appDir, "../../../.."));
        var projectDir = Path.Combine(sourceDir, "LightKit");
        var controlsDir = Path.Combine(projectDir, "Controls");
        var designControlsDir = Path.Combine(controlsDir, "Design");
        var baseControlsDir = Path.Combine(controlsDir, "Base");
        var unSupportedControlsDir = Path.Combine(baseControlsDir, "Unsupported");
        var themeFile = Path.Combine(projectDir, "Themes", "BaseTheme.axaml");

        CreateThemeFile(themeFile, projectDir, designControlsDir, baseControlsDir, unSupportedControlsDir, controlsDir);
    }

    static void CreateThemeFile(string themeFile, string projectDir, string designControlsDir, string baseControlsDir, string unSupportedControlsDir, string controlsDir)
    {
        using var writer = new StreamWriter(themeFile, false);

        writer.WriteLine("<Styles");
        writer.WriteLine("    x:Class=\"LightKit.Themes.BaseTheme\"");
        writer.WriteLine("    xmlns=\"https://github.com/avaloniaui\"");
        writer.WriteLine("    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">");
        writer.WriteLine("    <Styles.Resources>");
        writer.WriteLine("        <ResourceDictionary>");
        writer.WriteLine("            <ResourceDictionary.ThemeDictionaries>");
        writer.WriteLine("                <ResourceInclude x:Key=\"Default\" Source=\"@Light.axaml\" />");
        writer.WriteLine("                <ResourceInclude x:Key=\"Light\" Source=\"@Light.axaml\" />");
        writer.WriteLine("                <ResourceInclude x:Key=\"Dark\" Source=\"@Dark.axaml\" />");
        writer.WriteLine("            </ResourceDictionary.ThemeDictionaries>");
        writer.WriteLine("            <ResourceDictionary.MergedDictionaries>");

        writer.WriteLine("                <!-- Design -->");
        foreach (var xamlFile in GetXamlFiles(designControlsDir))
            writer.WriteLine($"                <MergeResourceInclude Source=\"../{Path.GetRelativePath(projectDir, xamlFile).Replace('\\', '/')}\" />");

        writer.WriteLine("                <!-- Unsupported Controls -->");
        foreach (var xamlFile in GetXamlFiles(unSupportedControlsDir))
            writer.WriteLine($"                <MergeResourceInclude Source=\"../{Path.GetRelativePath(projectDir, xamlFile).Replace('\\', '/')}\" />");

        writer.WriteLine("                <!-- Base Controls -->");
        foreach (var xamlFile in GetXamlFiles(baseControlsDir))
            writer.WriteLine($"                <MergeResourceInclude Source=\"../{Path.GetRelativePath(projectDir, xamlFile).Replace('\\', '/')}\" />");

        writer.WriteLine("                <!-- Controls -->");
        foreach (var xamlFile in GetXamlFiles(controlsDir))
            writer.WriteLine($"                <MergeResourceInclude Source=\"../{Path.GetRelativePath(projectDir, xamlFile).Replace('\\', '/')}\" />");

        writer.WriteLine("            </ResourceDictionary.MergedDictionaries>");
        writer.WriteLine("        </ResourceDictionary>");
        writer.WriteLine("    </Styles.Resources>");
        writer.WriteLine("</Styles>");
    }

    static IReadOnlyCollection<string> GetXamlFiles(string directory)
    {
        var files = new List<string>();
        foreach (var file in Directory.GetFiles(directory, "*.axaml", SearchOption.TopDirectoryOnly))
            files.Add(file);

        files.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a, b));

        return files;
    }
}
