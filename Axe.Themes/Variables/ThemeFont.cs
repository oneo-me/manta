using Avalonia.Media;
using Axe.Themes.Base;

namespace Axe.Themes.Variables;

public enum ThemeFontKey
{
    Sans,
    Serif,
    Mono
}

public sealed class ThemeFontKeyExtension(ThemeFontKey key) : ThemeKeyBase(key);

public sealed class ThemeFont(ThemeFontKey key) : ThemeValueBase
{
    public ThemeFontKey Key { get; } = key;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }

    protected override object OnConvert(IList<object?> values)
    {
        var value = values[0];
        if (value is not FontFamily fontFamily)
            return FontFamily.Default;

        return fontFamily;
    }
}
