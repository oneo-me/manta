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
    BorderBrush,
    Foreground,
    ColorForeground,

    Primary,
    Success,
    Warning,
    Danger
}

public enum MBrushType
{
    Normal,

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
    public MBrushType Type { get; set; } = MBrushType.Normal;
    public double Opacity { get; set; } = 1.0;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }

    static int ColorType2Step(MBrushType colorType)
    {
        return colorType switch
        {
            MBrushType.Dark3  => 0,
            MBrushType.Dark2  => 1,
            MBrushType.Dark1  => 2,
            MBrushType.Normal => 3,
            MBrushType.Light1 => 4,
            MBrushType.Light2 => 5,
            MBrushType.Light3 => 6,
            _                  => throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null)
        };
    }

    public static ColorBase? GetColor(object? value, MBrushType colorType)
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
