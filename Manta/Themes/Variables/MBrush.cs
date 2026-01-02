using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Manta.Themes.Base;
using Manta.Themes.Values;

namespace Manta.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MBrushKey
{
    Background,
    Foreground,
    Card,
    CardForeground,
    Popover,
    PopoverForeground,
    Primary,
    PrimaryForeground,
    Secondary,
    SecondaryForeground,
    Muted,
    MutedForeground,
    Accent,
    AccentForeground,
    Destructive,
    DestructiveForeground,
    Border,
    Input,
    Ring,
    Chart1,
    Chart2,
    Chart3,
    Chart4,
    Chart5,
    SidebarBackground,
    SidebarForeground,
    SidebarPrimary,
    SidebarPrimaryForeground,
    SidebarAccent,
    SidebarAccentForeground,
    SidebarBorder,
    SidebarRing,
    Shadow
}

public enum MBrushType
{
    Default,

    Light1,
    Light2,
    Light3,

    Dark1,
    Dark2,
    Dark3
}

public sealed class MBrushKeyExtension(MBrushKey key) : MKeyBase(key);

public sealed class MBrush(MBrushKey key) : MValueBase
{
    public MBrushKey Key { get; } = key;
    public MBrushType Type { get; set; } = MBrushType.Default;
    public double Opacity { get; set; } = 1.0;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }

    public static ColorBase? GetColor(object? value, MBrushType colorType)
    {
        if (value is ColorBase colorBase)
            return colorBase;

        return null;
    }

    static OklchColor? GetOklchColor(object? value, MBrushType colorType)
    {
        OklchColor? color = null;

        if (value is RgbColor rgbColor)
            color = rgbColor.ToOklchColor();
        else if (value is OklchColor oklchColor)
            color = oklchColor;

        if (color is null)
            return null;

        switch (colorType)
        {
            case MBrushType.Default:
                return color;

            case MBrushType.Light1:
                return new()
                {
                    Opacity = color.Opacity,
                    L = Math.Min(color.L + 0.1, 1.0),
                    C = color.C,
                    H = color.H
                };

            case MBrushType.Light2:
                return new()
                {
                    Opacity = color.Opacity,
                    L = Math.Min(color.L + 0.2, 1.0),
                    C = color.C,
                    H = color.H
                };

            case MBrushType.Light3:
                return new()
                {
                    Opacity = color.Opacity,
                    L = Math.Min(color.L + 0.3, 1.0),
                    C = color.C,
                    H = color.H
                };

            case MBrushType.Dark1:
                return new()
                {
                    Opacity = color.Opacity,
                    L = Math.Max(color.L - 0.1, 0.0),
                    C = color.C,
                    H = color.H
                };

            case MBrushType.Dark2:
                return new()
                {
                    Opacity = color.Opacity,
                    L = Math.Max(color.L - 0.2, 0.0),
                    C = color.C,
                    H = color.H
                };

            case MBrushType.Dark3:
                return new()
                {
                    Opacity = color.Opacity,
                    L = Math.Max(color.L - 0.3, 0.0),
                    C = color.C,
                    H = color.H
                };

            default:
                throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null);
        }
    }

    protected override object ProcessValues(IList<object?> values)
    {
        var oklchColor = GetOklchColor(values[0], Type);
        if (oklchColor is null)
            return Brushes.Transparent;

        var avaloniaColor = oklchColor.ToColor(Opacity);
        return new ImmutableSolidColorBrush(avaloniaColor);
    }
}
