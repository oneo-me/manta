namespace LightKit.Prepare;

sealed class ThemeFile(Options options)
{
    static IReadOnlyCollection<string> GetXamlFiles(string directory)
    {
        var files = new List<string>();
        foreach (var file in Directory.GetFiles(directory, "*.axaml", SearchOption.TopDirectoryOnly))
            files.Add(file);

        files.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a, b));

        return files;
    }

    public void Prepare()
    {
        using var writer = new StreamWriter(options.BaseThemeFile, false);

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
        foreach (var xamlFile in GetXamlFiles(options.DesignControlsDir))
            writer.WriteLine($"                <MergeResourceInclude Source=\"../{Path.GetRelativePath(options.ProjectDir, xamlFile).Replace('\\', '/')}\" />");

        writer.WriteLine("                <!-- Supported Controls -->");
        foreach (var xamlFile in GetXamlFiles(options.SupportedControlsDir))
            writer.WriteLine($"                <MergeResourceInclude Source=\"../{Path.GetRelativePath(options.ProjectDir, xamlFile).Replace('\\', '/')}\" />");

        writer.WriteLine("                <!-- Unsupported Controls -->");
        foreach (var xamlFile in GetXamlFiles(options.UnSupportedControlsDir))
            writer.WriteLine($"                <MergeResourceInclude Source=\"../{Path.GetRelativePath(options.ProjectDir, xamlFile).Replace('\\', '/')}\" />");

        writer.WriteLine("                <!-- Base Controls -->");
        foreach (var xamlFile in GetXamlFiles(options.BaseControlsDir))
            writer.WriteLine($"                <MergeResourceInclude Source=\"../{Path.GetRelativePath(options.ProjectDir, xamlFile).Replace('\\', '/')}\" />");

        writer.WriteLine("                <!-- Controls -->");
        foreach (var xamlFile in GetXamlFiles(options.ControlsDir))
            writer.WriteLine($"                <MergeResourceInclude Source=\"../{Path.GetRelativePath(options.ProjectDir, xamlFile).Replace('\\', '/')}\" />");

        writer.WriteLine("            </ResourceDictionary.MergedDictionaries>");
        writer.WriteLine("        </ResourceDictionary>");
        writer.WriteLine("    </Styles.Resources>");
        writer.WriteLine("</Styles>");
    }
}
