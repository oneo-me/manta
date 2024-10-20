using System.Diagnostics.CodeAnalysis;
using AirKit.Themes.Base;
using Avalonia.Media;

namespace AirKit.Themes;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeShadowKey
{
    None,

    Shadow1,
    Shadow2,
    Shadow3,
    Shadow4,
    Shadow5,
    Shadow6,

    InnerShadow1,
    InnerShadow2,
    InnerShadow3,
    InnerShadow4,
    InnerShadow5,
    InnerShadow6,

    Border1,
    Border2,
    Border3,
    Border4,
    Border5,
    Border6,

    InnerBorder1,
    InnerBorder2,
    InnerBorder3,
    InnerBorder4,
    InnerBorder5,
    InnerBorder6
}

public sealed class ThemeShadowKeyExtension(ThemeShadowKey key) : ThemeKeyBase(key);

public sealed class ThemeShadow(ThemeShadowKey key) : ThemeValueBase
{
    protected internal override bool IsDynamic => true;

    protected override IEnumerable<object> GetKeys()
    {
        yield return key;
    }

    protected internal override object OnConvert(IList<object?> values)
    {
        var value0 = values[0];

        if (value0 is BoxShadows boxShadows)
            return boxShadows;

        return default(BoxShadows);
    }
}
