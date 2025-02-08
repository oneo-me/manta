using System.Diagnostics.CodeAnalysis;
using AirKit.Themes.Base;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace AirKit.Themes.Values;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeBrushKey
{
    Shadow,
    MaskBrush,

    Primary,
    Success,
    Warning,
    Error,

    Primary_Foreground,
    Success_Foreground,
    Warning_Foreground,
    Error_Foreground,

    Window_Background,

    Background,
    Foreground,
    BorderBrush,

    Flyout_Background,
    Flyout_Foreground,
    Flyout_BorderBrush
}

public sealed class ThemeBrushKeyExtension(ThemeBrushKey key) : ThemeKeyBase(key);

public sealed class ThemeBrush(ThemeBrushKey key) : ThemeValueBase
{
    public double Opacity { get; set; } = 1;
    public bool ResetOpacity { get; set; }

    protected override IEnumerable<object> GetKeys()
    {
        yield return key;
    }

    protected override object OnConvert(IList<object?> values)
    {
        var value0 = values[0];

        var color = GetColor(value0);
        if (color == Colors.Transparent)
            return Brushes.Transparent;

        var opacity = ResetOpacity ? 255d * Opacity : color.A * Opacity;
        var result = new Color((byte)opacity, color.R, color.G, color.B);

        return new ImmutableSolidColorBrush(result);
    }

    static Color GetColor(object? value)
    {
        return value switch
        {
            Color color                     => color,
            SolidColorBrush solidColorBrush => new((byte)(solidColorBrush.Color.A * solidColorBrush.Opacity), solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B),
            _                               => Colors.Transparent
        };
    }
}
