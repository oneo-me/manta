using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using Manta.Themes.Base;

namespace Manta.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MRingKey
{
    None,

    XS,
    SM,
    MD,
    LG,
    XL,
    XL2
}

public sealed class MRingKeyExtension(MRingKey key) : MKeyBase(key);

public sealed class MRing(MRingKey key) : MValueBase
{
    public MRingKey Key { get; } = key;
    public MBrushKey BrushKey { get; init; } = MBrushKey.Primary;
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
                IsInset = false,
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
