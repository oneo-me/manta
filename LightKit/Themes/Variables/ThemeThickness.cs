using System.Diagnostics.CodeAnalysis;
using Avalonia;
using LightKit.Themes.Extensions;
using LightKit.Themes.Base;

namespace LightKit.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeThicknessKey
{
    Normal
}

[Flags]
public enum ThicknessFilter
{
    None = 0,

    Left = 1 << 0,
    Right = 1 << 1,
    Top = 1 << 2,
    Bottom = 1 << 3,

    All = Left | Right | Top | Bottom
}

public sealed class ThemeThicknessKeyExtension(ThemeThicknessKey key) : ThemeKeyBase(key);

public sealed class ThemeThickness(ThemeThicknessKey key) : ThemeValueBase
{
    ThicknessFilter _filter = ThicknessFilter.All;

    public string Filter
    {
        set => value.TryParse(out _filter);
    }

    protected override IEnumerable<object> GetKeys()
    {
        yield return key;
    }

    protected override object ProcessValues(IList<object?> values)
    {
        var value = values[0];
        var size = 0d;

        if (value is double number)
            size = number;

        return new Thickness(
            _filter.HasFlag(ThicknessFilter.Left, size),
            _filter.HasFlag(ThicknessFilter.Top, size),
            _filter.HasFlag(ThicknessFilter.Right, size),
            _filter.HasFlag(ThicknessFilter.Bottom, size)
        );
    }
}
