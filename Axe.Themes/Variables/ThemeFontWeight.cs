using Avalonia.Media;
using Axe.Themes.Base;

namespace Axe.Themes.Variables;

public enum ThemeFontWeightKey
{
    Thin,
    Extralight,
    Light,
    Normal,
    Medium,
    Semibold,
    Bold,
    Extrabold,
    Black
}

public sealed class ThemeFontWeightKeyExtension(ThemeFontWeightKey key) : ThemeKeyBase(key);

public sealed class ThemeFontWeight(ThemeFontWeightKey key) : ThemeValueBase
{
    public ThemeFontWeightKey Key { get; } = key;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }

    protected override object ProcessValues(IList<object?> values)
    {
        var value = values[0];
        if (value is not FontWeight fontWeight)
            return FontWeight.Normal;

        return fontWeight;
    }
}
