using System.Diagnostics.CodeAnalysis;
using AirKit.Themes.Base;
using Avalonia;

namespace AirKit.Themes;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeRadiusKey
{
    Normal,
    Flyout,
    Dialog
}

public sealed class ThemeRadiusKeyExtension(ThemeRadiusKey key) : ThemeKeyBase(key);

public sealed class ThemeRadius(ThemeRadiusKey key) : ThemeValueBase
{
    public double Offset { get; set; }
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

        topLeft += Offset;
        topRight += Offset;
        bottomRight += Offset;
        bottomLeft += Offset;

        return new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
    }
}
