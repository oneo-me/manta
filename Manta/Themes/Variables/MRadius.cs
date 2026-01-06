using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Manta.Extensions;
using Manta.Themes.Base;

namespace Manta.Themes.Variables;

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

public sealed class MRadius(MRadiusVariant variant) : MValueBase
{
    public MRadiusVariant Variant { get; } = variant;

    MRadiusFilter _filter = MRadiusFilter.All;

    public string Filter
    {
        set => value.TryParseEnum(out _filter, '|');
    }

    protected override IEnumerable<object> GetKeys()
    {
        yield return MSizeKey.Radius;
    }

    static readonly double[] _multipliers =
    [
        0, 0.25, 0.5, 0.75, 1.0, 1.5, 2.0, 3.0, 4.0, -1
    ];

    protected override object ProcessValues(IList<object?> values)
    {
        var size = 0d;
        if (values[0] is double number)
            size = number;

        var multiplier = _multipliers[(int)Variant];
        var newSize = size * multiplier;

        if (newSize <= 0d)
            return default(CornerRadius);

        return new CornerRadius(
            _filter.HasFlag(MRadiusFilter.LeftTop, newSize),
            _filter.HasFlag(MRadiusFilter.RightTop, newSize),
            _filter.HasFlag(MRadiusFilter.RightBottom, newSize),
            _filter.HasFlag(MRadiusFilter.LeftBottom, newSize)
        );
    }
}
