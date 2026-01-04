using Manta.Tools;

namespace Manta.Prepare;

sealed class ThemeParser(Options options)
{
    public void Parse()
    {
        var cssLines = File.ReadAllLines(options.ThemeSourceFile);
        var result = TweakcnParser.Parser(cssLines);
        var lightStr = result.LightTheme.ToXaml(string.Empty, false);
        var darkStr = result.DarkTheme.ToXaml(lightStr, false);
        File.WriteAllText(options.LightThemeFile, lightStr);
        File.WriteAllText(options.DarkThemeFile, darkStr);
    }
}
