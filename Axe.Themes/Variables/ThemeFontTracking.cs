using Axe.Themes.Base;

namespace Axe.Themes.Variables;

public enum ThemeFontTrackingKey
{
    Tighter,
    Tight,
    Normal,
    Wide,
    Wider,
    Widest
}

public sealed class ThemeFontTrackingKeyExtension(ThemeFontTrackingKey key) : ThemeKeyBase(key);

public sealed class ThemeFontTracking(ThemeFontTrackingKey key) : ThemeValueBase
{
    public ThemeFontTrackingKey Key { get; } = key;

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
