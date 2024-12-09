using System.Diagnostics.CodeAnalysis;
using AirKit.Themes.Base;
using AirKit.Themes.Extensions;
using Avalonia;

namespace AirKit.Themes.Values;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeThicknessKey
{
    Normal,
    Flyout,
    Dialog,

    Padding,
    Menu_Padding,
    Button_Padding,
    TextBox_Padding,
    TabItem_Padding,

    Shadow0,
    Shadow1,
    Shadow2,
    Shadow3,
    Shadow4,
    Shadow5,
    Shadow6
}

[Flags]
public enum ThicknessFilter
{
    None = 0,

    Left = 1 << 0,
    Top = 1 << 1,
    Right = 1 << 2,
    Bottom = 1 << 3,

    All = Left | Top | Right | Bottom
}

public sealed class ThemeThicknessKeyExtension(ThemeThicknessKey key) : ThemeKeyBase(key);

public sealed class ThemeThickness(ThemeThicknessKey key) : ThemeValueBase
{
    ThicknessFilter _filter = ThicknessFilter.All;

    public string Filter
    {
        set => _filter = value.Parse<ThicknessFilter>();
    }

    public double LeftOffset { get; set; }
    public double RightOffset { get; set; }
    public double TopOffset { get; set; }
    public double BottomOffset { get; set; }

    public double Offset
    {
        set
        {
            LeftRightOffset = value;
            TopBottomOffset = value;
        }
    }

    public double LeftRightOffset
    {
        set
        {
            LeftOffset = value;
            RightOffset = value;
        }
    }

    public double TopBottomOffset
    {
        set
        {
            TopOffset = value;
            BottomOffset = value;
        }
    }

    public bool Reverse { get; set; }

    protected override IEnumerable<object> GetKeys()
    {
        yield return key;
    }

    protected override object OnConvert(IList<object?> values)
    {
        var value0 = values[0];

        var left = 0d;
        var top = 0d;
        var right = 0d;
        var bottom = 0d;

        if (value0 is double value)
        {
            left = value;
            top = value;
            right = value;
            bottom = value;
        }
        else if (value0 is Thickness thickness)
        {
            left = thickness.Left;
            top = thickness.Top;
            right = thickness.Right;
            bottom = thickness.Bottom;
        }

        left += LeftOffset;
        right += RightOffset;
        top += TopOffset;
        bottom += BottomOffset;

        if (Reverse)
        {
            left = -left;
            right = -right;
            top = -top;
            bottom = -bottom;
        }

        return new Thickness(
            _filter.HasFlag(ThicknessFilter.Left, left),
            _filter.HasFlag(ThicknessFilter.Top, top),
            _filter.HasFlag(ThicknessFilter.Right, right),
            _filter.HasFlag(ThicknessFilter.Bottom, bottom)
        );
    }
}
