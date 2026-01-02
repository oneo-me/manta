using Manta.Tools;

namespace Manta.Prepare;

sealed class ThemeParser(Options options)
{
    public void Parse()
    {
        var result = TweakcnParser.Parser(options.ThemeSourceFile);
        File.WriteAllText(options.LightThemeFile, result.LightTheme.ToXaml());
        File.WriteAllText(options.DarkThemeFile, result.DarkTheme.ToXaml());
    }
}
