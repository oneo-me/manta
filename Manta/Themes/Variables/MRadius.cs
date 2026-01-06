using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Manta.Extensions;
using Manta.Themes.Base;

namespace Manta.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MRadiusKey
{
    Radius
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MRadiusVariant
{
    None,

    XS,
    SM,
    MD,
    LG,
    XL,
    XL2,
    XL3,
    XL4,

    Full
}

[Flags]
public enum MRadiusFilter
{
    None = 0,

    LeftTop = 1 << 0,
    RightTop = 1 << 1,
    LeftBottom = 1 << 2,
    RightBottom = 1 << 3,

    Left = LeftTop | LeftBottom,
    Right = RightTop | RightBottom,
    Top = LeftTop | RightTop,
    Bottom = LeftBottom | RightBottom,

    All = LeftTop | RightTop | LeftBottom | RightBottom
}

public sealed class MRadiusKeyExtension(MRadiusKey key) : MKeyBase(key);

public sealed class MRadius(MRadiusVariant variant) : MValueBase
{
    public MRadiusKey Key { get; set; } = MRadiusKey.Radius;
    public MRadiusVariant Variant { get; } = variant;

    MRadiusFilter _filter = MRadiusFilter.All;

    public string Filter
    {
        set => value.TryParseEnum(out _filter, '|');
    }

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }

    protected override object ProcessValues(IList<object?> values)
    {
        var size = 0d;
        if (Variant == MRadiusVariant.None)
            size = 0d;
        else if (Variant == MRadiusVariant.Full)
            size = 9999d;
        else if (values[0] is double number)
            size = number;

        if (size == 0d)
            return default(CornerRadius);

        var newSize = size * GetScalarFromVariant(Variant);

        return new CornerRadius(
            _filter.HasFlag(MRadiusFilter.LeftTop, newSize),
            _filter.HasFlag(MRadiusFilter.RightTop, newSize),
            _filter.HasFlag(MRadiusFilter.RightBottom, newSize),
            _filter.HasFlag(MRadiusFilter.LeftBottom, newSize)
        );
    }

    static double GetScalarFromVariant(MRadiusVariant mRadiusVariant)
    {
        return mRadiusVariant switch
        {
            MRadiusVariant.XS  => 0.25,
            MRadiusVariant.SM  => 0.5,
            MRadiusVariant.MD  => 0.75,
            MRadiusVariant.LG  => 1.0,
            MRadiusVariant.XL  => 1.5,
            MRadiusVariant.XL2 => 2.0,
            MRadiusVariant.XL3 => 3.0,
            MRadiusVariant.XL4 => 4.0,

            _ => 1.0
        };
    }
}
