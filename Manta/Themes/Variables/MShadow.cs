using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using Manta.Themes.Base;

namespace Manta.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MShadowKey
{
    None,

    XS,
    SM,
    MD,
    LG,
    XL,
    XXL
}

public sealed class MShadowKeyExtension(MShadowKey key) : MKeyBase(key);

public sealed class MShadow(MShadowKey key) : MValueBase
{
    public MShadowKey Key { get; } = key;
    public bool IsInset { get; init; }
    public MBrushKey BrushKey { get; init; } = MBrushKey.Shadow;
    public MBrushType BrushType { get; init; } = MBrushType.Default;
    public double BrushOpacity { get; init; } = -1;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
        yield return BrushKey;
    }

    protected override object ProcessValues(IList<object?> values)
    {
        var value0 = values[0];
        var value1 = MBrush.GetOklchColor(values[1], BrushType);

        if (value0 is not BoxShadows boxShadows || value1 is null)
            return default(BoxShadows);

        var shadows = new BoxShadow[boxShadows.Count];

        for (var i = 0; i < boxShadows.Count; i++)
        {
            var boxShadow = boxShadows[i];

            shadows[i] = new()
            {
                OffsetX = boxShadow.OffsetX,
                OffsetY = boxShadow.OffsetY,
                Blur = boxShadow.Blur,
                Spread = boxShadow.Spread,
                IsInset = IsInset,
                Color = value1.ToColor(BrushOpacity >= 0 ? BrushOpacity : boxShadow.Color.A / 255d)
            };
        }

        return shadows.Length switch
        {
            0 => default(BoxShadows),
            1 => new(shadows[0]),
            _ => new(shadows[0], shadows.Skip(1).ToArray())
        };
    }
}
