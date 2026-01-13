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
    Shadow,
    Desgin
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

    static double GetTypeValue(MBrushType colorType)
    {
        return colorType switch
        {
            MBrushType.Default => 0.0,
            MBrushType.Light1  => 0.1,
            MBrushType.Light2  => 0.2,
            MBrushType.Light3  => 0.3,
            MBrushType.Dark1   => -0.1,
            MBrushType.Dark2   => -0.2,
            MBrushType.Dark3   => -0.3,
            _                  => throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null)
        };
    }

    internal static OklchColor? GetOklchColor(object? value, MBrushType colorType)
    {
        OklchColor? color = null;

        if (value is RgbColor rgbColor)
            color = rgbColor.ToOklchColor();
        else if (value is OklchColor oklchColor)
            color = oklchColor;

        if (color is null)
            return null;

        return new()
        {
            Opacity = color.Opacity,
            L = Math.Max(color.L + GetTypeValue(colorType), 0.0),
            C = color.C,
            H = color.H
        };
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
