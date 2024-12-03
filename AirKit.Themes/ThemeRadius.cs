using System.Diagnostics.CodeAnalysis;
using AirKit.Themes.Base;
using AirKit.Themes.Extensions;
using Avalonia;

namespace AirKit.Themes;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeRadiusKey
{
    Normal,
    Flyout,
    Dialog
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
        set => _filter = value.Parse<RadiusFilter>();
    }

    public double LeftTopOffset { get; set; }
    public double RightTopOffset { get; set; }
    public double LeftBottomOffset { get; set; }
    public double RightBottomOffset { get; set; }

    public double Offset
    {
        set
        {
            LeftTopOffset = value;
            RightTopOffset = value;
            LeftBottomOffset = value;
            RightBottomOffset = value;
        }
    }

    public double LeftOffset
    {
        set
        {
            LeftTopOffset = value;
            LeftBottomOffset = value;
        }
    }

    public double RightOffset
    {
        set
        {
            RightTopOffset = value;
            RightBottomOffset = value;
        }
    }

    public double TopOffset
    {
        set
        {
            LeftTopOffset = value;
            RightTopOffset = value;
        }
    }

    public double BottomOffset
    {
        set
        {
            LeftBottomOffset = value;
            RightBottomOffset = value;
        }
    }

    protected internal override bool IsDynamic => true;

    protected override IEnumerable<object> GetKeys()
    {
        yield return key;
    }

    protected internal override object OnConvert(IList<object?> values)
    {
        var value0 = values[0];

        var topLeft = 0d;
        var topRight = 0d;
        var bottomRight = 0d;
        var bottomLeft = 0d;

        if (value0 is double value)
        {
            topLeft = topRight = bottomRight = bottomLeft = value;
        }
        else if (value0 is CornerRadius cornerRadius)
        {
            topLeft = cornerRadius.TopLeft;
            topRight = cornerRadius.TopRight;
            bottomRight = cornerRadius.BottomRight;
            bottomLeft = cornerRadius.BottomLeft;
        }

        topLeft += LeftTopOffset;
        topRight += RightTopOffset;
        bottomLeft += LeftBottomOffset;
        bottomRight += RightBottomOffset;

        return new CornerRadius(
            _filter.HasFlag(RadiusFilter.LeftTop, topLeft),
            _filter.HasFlag(RadiusFilter.RightTop, topRight),
            _filter.HasFlag(RadiusFilter.RightBottom, bottomRight),
            _filter.HasFlag(RadiusFilter.LeftBottom, bottomLeft)
        );
    }
}
