using Manta.Tools.Extensions;
using Manta.Tools.Themes;

namespace Manta.Tools;

public static class TweakcnParser
{
    static readonly Dictionary<string, Action<ThemeVariable, string>> _functions = new()
    {
        { "background", (theme, value) => theme.Background = value.ParserColor() },
        { "foreground", (theme, value) => theme.Foreground = value.ParserColor() },
        { "card", (theme, value) => theme.Card = value.ParserColor() },
        { "card-foreground", (theme, value) => theme.CardForeground = value.ParserColor() },
        { "popover", (theme, value) => theme.Popover = value.ParserColor() },
        { "popover-foreground", (theme, value) => theme.PopoverForeground = value.ParserColor() },
        { "primary", (theme, value) => theme.Primary = value.ParserColor() },
        { "primary-foreground", (theme, value) => theme.PrimaryForeground = value.ParserColor() },
        { "secondary", (theme, value) => theme.Secondary = value.ParserColor() },
        { "secondary-foreground", (theme, value) => theme.SecondaryForeground = value.ParserColor() },
        { "muted", (theme, value) => theme.Muted = value.ParserColor() },
        { "muted-foreground", (theme, value) => theme.MutedForeground = value.ParserColor() },
        { "accent", (theme, value) => theme.Accent = value.ParserColor() },
        { "accent-foreground", (theme, value) => theme.AccentForeground = value.ParserColor() },
        { "destructive", (theme, value) => theme.Destructive = value.ParserColor() },
        { "destructive-foreground", (theme, value) => theme.DestructiveForeground = value.ParserColor() },
        { "border", (theme, value) => theme.Border = value.ParserColor() },
        { "input", (theme, value) => theme.Input = value.ParserColor() },
        { "ring", (theme, value) => theme.Ring = value.ParserColor() },
        { "chart-1", (theme, value) => theme.Chart1 = value.ParserColor() },
        { "chart-2", (theme, value) => theme.Chart2 = value.ParserColor() },
        { "chart-3", (theme, value) => theme.Chart3 = value.ParserColor() },
        { "chart-4", (theme, value) => theme.Chart4 = value.ParserColor() },
        { "chart-5", (theme, value) => theme.Chart5 = value.ParserColor() },
        { "sidebar", (theme, value) => theme.SidebarBackground = value.ParserColor() },
        { "sidebar-foreground", (theme, value) => theme.SidebarForeground = value.ParserColor() },
        { "sidebar-primary", (theme, value) => theme.SidebarPrimary = value.ParserColor() },
        { "sidebar-primary-foreground", (theme, value) => theme.SidebarPrimaryForeground = value.ParserColor() },
        { "sidebar-accent", (theme, value) => theme.SidebarAccent = value.ParserColor() },
        { "sidebar-accent-foreground", (theme, value) => theme.SidebarAccentForeground = value.ParserColor() },
        { "sidebar-border", (theme, value) => theme.SidebarBorder = value.ParserColor() },
        { "sidebar-ring", (theme, value) => theme.SidebarRing = value.ParserColor() },
        { "shadow-color", (theme, value) => theme.Shadow = value.ParserColor() },
        { "shadow-2xs", (theme, value) => theme.XXS = value.ParseShadow() },
        { "shadow-xs", (theme, value) => theme.XS = value.ParseShadow() },
        { "shadow-sm", (theme, value) => theme.SM = value.ParseShadow() },
        { "shadow", (theme, value) => theme.Default = value.ParseShadow() },
        { "shadow-md", (theme, value) => theme.MD = value.ParseShadow() },
        { "shadow-lg", (theme, value) => theme.LG = value.ParseShadow() },
        { "shadow-xl", (theme, value) => theme.XL = value.ParseShadow() },
        { "shadow-2xl", (theme, value) => theme.XXL = value.ParseShadow() },
        { "radius", (theme, value) => theme.Radius = value.ParserNumber() },
        { "spacing", (theme, value) => theme.Spacing = value.ParserNumber() }
    };

    public static TweakcnParserResult Parser(string[] cssLines)
    {
        var themeSetter = new ThemeSetter();

        foreach (var line in cssLines)
        {
            var trimmedLine = line.Trim();
            if (trimmedLine == ":root {")
            {
                themeSetter.IsLight = true;
            }
            else if (trimmedLine == ".dark {")
            {
                themeSetter.IsLight = false;
            }
            else if (trimmedLine == "@theme inline {")
            {
                break;
            }
            else if (trimmedLine.StartsWith("--"))
            {
                var parts = trimmedLine[2..].Split(':', 2);
                if (parts.Length != 2)
                    continue;

                var name = parts[0].Trim();
                var value = parts[1].Trim().TrimEnd(';');

                if (_functions.TryGetValue(name, out var func))
                    func(themeSetter.Current, value);
            }
        }

        return new()
        {
            LightTheme = themeSetter.Light,
            DarkTheme = themeSetter.Dark
        };
    }
}
