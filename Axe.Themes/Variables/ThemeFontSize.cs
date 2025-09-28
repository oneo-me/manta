using System.Diagnostics.CodeAnalysis;
using Axe.Themes.Base;

namespace Axe.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeFontSizeKey
{
    SM,
    Base,
    LG,
    XL,
    XL2,
    XL3,
    XL4,
    XL5,
    XL6,
    XL7,
    XL8,
    XL9
}

public sealed class ThemeFontSizeKeyExtension(ThemeFontSizeKey key) : ThemeKeyBase(key);

public sealed class ThemeFontSize(ThemeFontSizeKey key) : ThemeValueBase
{
    public ThemeFontSizeKey Key { get; } = key;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }

    protected override object ProcessValues(IList<object?> values)
    {
        var value = values[0];
        if (value is not double number)
            return 0d;

        return number;
    }
}
