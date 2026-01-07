using System.Diagnostics.CodeAnalysis;
using Avalonia;
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
    XL2
}

public enum MShadowValueType
{
    Shadow,
    InsetShadow,
    Margin,
    MarginNegative
}

public sealed class MShadowKeyExtension(MShadowKey key) : MKeyBase(key);

public sealed class MShadow(MShadowKey key) : MValueBase
{
    public MShadowKey Key { get; } = key;
    public MShadowValueType ValueType { get; init; } = MShadowValueType.Shadow;
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
            switch (ValueType)
            {
                case MShadowValueType.Shadow:
                case MShadowValueType.InsetShadow:
                    return default(BoxShadows);

                case MShadowValueType.Margin:
                case MShadowValueType.MarginNegative:
                    return default(Thickness);

                default:
                    throw new ArgumentOutOfRangeException();
            }

        switch (ValueType)
        {
            case MShadowValueType.Shadow:
            case MShadowValueType.InsetShadow:
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
                        IsInset = ValueType == MShadowValueType.InsetShadow,
                        Color = value1.ToColor(BrushOpacity >= 0 ? BrushOpacity : boxShadow.Color.A / 255d)
                    };
                }

                return shadows.Length switch
                {
                    0 => default(BoxShadows),
                    1 => new(shadows[0]),
                    _ => new(shadows[0], shadows.Skip(1).ToArray())
                };

            case MShadowValueType.Margin:
            case MShadowValueType.MarginNegative:
                var left = 0d;
                var top = 0d;
                var right = 0d;
                var bottom = 0d;

                foreach (var boxShadow in boxShadows)
                {
                    var blur = boxShadow.Blur;
                    var spread = boxShadow.Spread;
                    var offsetX = boxShadow.OffsetX;
                    var offsetY = boxShadow.OffsetY;

                    left = Math.Max(left, blur + spread - offsetX);
                    top = Math.Max(top, blur + spread - offsetY);
                    right = Math.Max(right, blur + spread + offsetX);
                    bottom = Math.Max(bottom, blur + spread + offsetY);
                }

                if (ValueType == MShadowValueType.MarginNegative)
                {
                    left = -left;
                    top = -top;
                    right = -right;
                    bottom = -bottom;
                }

                return new Thickness(left, top, right, bottom);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
