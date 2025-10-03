using Axe.Themes.Base;

namespace Axe.Themes.Variables;

public enum ThemeFontSpacingKey
{
    Tighter,
    Tight,
    Normal,
    Wide,
    Wider,
    Widest
}

public sealed class ThemeFontSpacingKeyExtension(ThemeFontSpacingKey key) : ThemeKeyBase(key);

public sealed class ThemeFontSpacing(ThemeFontSpacingKey key) : ThemeValueBase
{
    public ThemeFontSpacingKey Key { get; } = key;

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
