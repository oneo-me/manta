using Manta.Tools;

namespace Manta.Prepare;

sealed class ThemeParser(Options options)
{
    public void Parse()
    {
        var cssLines = File.ReadAllLines(options.ThemeSourceFile);
        var result = TweakcnParser.Parser(cssLines);
        File.WriteAllText(options.LightThemeFile, result.LightTheme.ToXaml());
        File.WriteAllText(options.DarkThemeFile, result.DarkTheme.ToXaml());
    }
}
