using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using LightKit.Themes.Base;
using LightKit.Themes.Values;

namespace LightKit.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeColorSetKey
{
    Background,
    BorderBrush,
    Foreground,
    ColorForeground,

    Primary,
    Success,
    Warning,
    Error
}

public enum ThemeColorType
{
    Normal,

    Light1,
    Light2,
    Light3,

    Dark1,
    Dark2,
    Dark3
}

public sealed class ThemeColorSetKeyExtension(ThemeColorSetKey key) : ThemeKeyBase(key);

public sealed class ThemeColorSet(ThemeColorSetKey key) : ThemeValueBase
{
    public ThemeColorSetKey Key { get; } = key;
    public ThemeColorType Type { get; set; } = ThemeColorType.Normal;
    public double Opacity { get; set; } = 1.0;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }

    static int ColorType2Step(ThemeColorType colorType)
    {
        return colorType switch
        {
            ThemeColorType.Dark3  => 0,
            ThemeColorType.Dark2  => 1,
            ThemeColorType.Dark1  => 2,
            ThemeColorType.Normal => 3,
            ThemeColorType.Light1 => 4,
            ThemeColorType.Light2 => 5,
            ThemeColorType.Light3 => 6,
            _                     => throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null)
        };
    }

    public static ColorBase? GetColor(object? value, ThemeColorType colorType)
    {
        if (value is ColorBase colorBase)
            return colorBase;

        if (value is ColorSet colorSet)
        {
            var step = ColorType2Step(colorType);
            var oklchColor = colorSet.GetColor(step, 7);
            return oklchColor;
        }

        return null;
    }

    protected override object ProcessValues(IList<object?> values)
    {
        if (values[0] is ColorBase colorBase)
        {
            var avaloniaColor = colorBase.ToColor(Opacity);
            return new ImmutableSolidColorBrush(avaloniaColor);
        }

        if (values[0] is ColorSet colorSet)
        {
            var step = ColorType2Step(Type);
            var oklchColor = colorSet.GetColor(step, 7);
            var avaloniaColor = oklchColor.ToColor(Opacity);

            return new ImmutableSolidColorBrush(avaloniaColor);
        }

        return Brushes.Transparent;
    }
}
