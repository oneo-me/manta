using System.Diagnostics.CodeAnalysis;
using Avalonia;
using LightKit.Themes.Extensions;
using LightKit.Themes.Base;

namespace LightKit.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeRadiusKey
{
    XS,
    SM,
    MD,
    LG,
    XL,
    XL2,
    XL3,
    XL4
}

[Flags]
public enum RadiusFilter
{
    None = 0,

    LeftTop = 1 << 0,
    RightTop = 1 << 1,
    LeftBottom = 1 << 2,
    RightBottom = 1 << 3,

    All = LeftTop | RightTop | LeftBottom | RightBottom
}

public sealed class ThemeRadiusKeyExtension(ThemeRadiusKey key) : ThemeKeyBase(key);

public sealed class ThemeRadius(ThemeRadiusKey key) : ThemeValueBase
{
    RadiusFilter _filter = RadiusFilter.All;

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

        return new CornerRadius(
            _filter.HasFlag(RadiusFilter.LeftTop, size),
            _filter.HasFlag(RadiusFilter.RightTop, size),
            _filter.HasFlag(RadiusFilter.RightBottom, size),
            _filter.HasFlag(RadiusFilter.LeftBottom, size)
        );
    }
}
