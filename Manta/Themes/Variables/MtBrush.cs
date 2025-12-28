using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Manta.Themes.Base;
using Manta.Themes.Values;

namespace Manta.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MtBrushKey
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

public enum MtBrushType
{
    Normal,

    Light1,
    Light2,
    Light3,

    Dark1,
    Dark2,
    Dark3
}

public sealed class MtBrushKeyExtension(MtBrushKey key) : MtKeyBase(key);

public sealed class MtBrush(MtBrushKey key) : MtValueBase
{
    public MtBrushKey Key { get; } = key;
    public MtBrushType Type { get; set; } = MtBrushType.Normal;
    public double Opacity { get; set; } = 1.0;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }

    static int ColorType2Step(MtBrushType colorType)
    {
        return colorType switch
        {
            MtBrushType.Dark3  => 0,
            MtBrushType.Dark2  => 1,
            MtBrushType.Dark1  => 2,
            MtBrushType.Normal => 3,
            MtBrushType.Light1 => 4,
            MtBrushType.Light2 => 5,
            MtBrushType.Light3 => 6,
            _                  => throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null)
        };
    }

    public static ColorBase? GetColor(object? value, MtBrushType colorType)
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
