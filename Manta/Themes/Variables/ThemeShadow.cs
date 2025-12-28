using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using Manta.Themes.Base;

namespace Manta.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeShadowKey
{
    XS2,
    XS,
    SM,
    MD,
    LG,
    XL,
    XL2,

    InsetXS2,
    InsetXS,
    InsetSM,

    DropXS,
    DropSM,
    DropMD,
    DropLG,
    DropXL,
    DropXL2,

    Menu
}

public sealed class ThemeShadowKeyExtension(ThemeShadowKey key) : ThemeKeyBase(key);

public sealed class ThemeShadow(ThemeShadowKey key) : ThemeValueBase
{
    public ThemeShadowKey Key { get; } = key;
    public ThemeColorSetKey ColorKey { get; init; } = ThemeColorSetKey.Foreground;
    public ThemeColorType ColorType { get; set; } = ThemeColorType.Normal;
    public double BrushOpacity { get; init; } = -1;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
        yield return ColorKey;
    }

    protected override object ProcessValues(IList<object?> values)
    {
        var value0 = values[0];
        var value1 = ThemeColorSet.GetColor(values[1], ColorType);

        if (value0 is not BoxShadows boxShadows || value1 is null)
            return default(BoxShadows);

        var shadows = new List<BoxShadow>();

        foreach (var boxShadow in boxShadows)
            shadows.Add(new()
            {
                OffsetX = boxShadow.OffsetX,
                OffsetY = boxShadow.OffsetY,
                Blur = boxShadow.Blur,
                Spread = boxShadow.Spread,
                IsInset = boxShadow.IsInset,
                Color = value1.ToColor(BrushOpacity > 0 ? BrushOpacity : boxShadow.Color.A / 255d)
            });

        if (shadows.Count == 0)
            return default(BoxShadows);

        if (shadows.Count == 1)
            return new BoxShadows(shadows[0]);

        return new BoxShadows(shadows[0], shadows.Skip(1).ToArray());
    }
}
