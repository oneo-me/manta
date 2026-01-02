using Manta.Tools.Themes;

namespace Manta.Tools;

public sealed class TweakcnParserResult
{
    public required ThemeVariable LightTheme { get; init; }
    public required ThemeVariable DarkTheme { get; init; }
}
