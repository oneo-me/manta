using Axe.Themes.Base;

namespace Axe.Themes.Variables;

public enum ThemeBrushRefKey
{
    WindowBackground,
    Background,
    Foreground,
    BorderBrush
}

public sealed class ThemeBrushRefKeyExtension(ThemeBrushRefKey key) : ThemeKeyBase(key);

public sealed class ThemeBrushRef(ThemeBrushRefKey key) : ThemeValueRefBase
{
    public ThemeBrushRefKey Key { get; } = key;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }
}
