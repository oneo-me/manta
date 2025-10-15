using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using LightKit.Themes.Base;
using LightKit.Themes.Values;

namespace LightKit.Themes.Variables;

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
    DropXL2
}

public sealed class ThemeShadowKeyExtension(ThemeShadowKey key) : ThemeKeyBase(key);

public sealed class ThemeShadow(ThemeShadowKey key) : ThemeValueBase
{
    public ThemeShadowKey Key { get; } = key;
    public ThemeBrushKey BrushKey { get; init; } = ThemeBrushKey.Black;
    public double BrushOpacity { get; init; } = -1;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
        yield return BrushKey;
    }

    protected override object ProcessValues(IList<object?> values)
    {
        var value0 = values[0];
        var value1 = values[1];

        if (value0 is not BoxShadows boxShadows)
            return default(BoxShadows);

        ColorBase? color = null;
        var opacity = 1d;
        if (value1 is ColorBase colorBase)
            color = colorBase;

        if (value1 is ThemeRefValue { Value: ImmutableSolidColorBrush immutableSolidColorBrush })
        {
            var refColor = immutableSolidColorBrush.Color;
            opacity = refColor.A / 255d * immutableSolidColorBrush.Opacity;
            color = new RgbColor
            {
                R = refColor.R,
                G = refColor.G,
                B = refColor.B
            };
        }

        if (color is null)
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
                Color = color.ToColor(opacity * (BrushOpacity > 0 ? BrushOpacity : 255d / boxShadow.Color.A))
            });

        if (shadows.Count == 0)
            return default(BoxShadows);

        if (shadows.Count == 1)
            return new BoxShadows(shadows[0]);

        return new BoxShadows(shadows[0], shadows.Skip(1).ToArray());
    }
}
