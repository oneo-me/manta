using System.Diagnostics.CodeAnalysis;
using Axe.Themes.Base;

namespace Axe.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeFontSizeKey
{
    XS,
    XS_LineHeight,
    SM,
    SM_LineHeight,
    Base,
    Base_LineHeight,
    LG,
    LG_LineHeight,
    XL,
    XL_LineHeight,
    XL2,
    XL2_LineHeight,
    XL3,
    XL3_LineHeight,
    XL4,
    XL4_LineHeight,
    XL5,
    XL5_LineHeight,
    XL6,
    XL6_LineHeight,
    XL7,
    XL7_LineHeight,
    XL8,
    XL8_LineHeight,
    XL9,
    XL9_LineHeight
}

public sealed class ThemeFontSizeKeyExtension(ThemeFontSizeKey key) : ThemeKeyBase(key);

public sealed class ThemeFontSize(ThemeFontSizeKey key) : ThemeValueBase
{
    public ThemeFontSizeKey Key { get; } = key;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }

    protected override object OnConvert(IList<object?> values)
    {
        var value = values[0];
        if (value is not double number)
            return 0d;

        return number;
    }
}
