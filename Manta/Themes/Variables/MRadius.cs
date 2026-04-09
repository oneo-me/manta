using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Manta.Extensions;
using Manta.Themes.Base;

namespace Manta.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MRadiusVariant
{
    /// <summary>
    ///     0
    /// </summary>
    None,

    /// <summary>
    ///     12 * 0.25 = 3
    /// </summary>
    XS,

    /// <summary>
    ///     12 * 0.5 = 6
    /// </summary>
    SM,

    /// <summary>
    ///     12 * 0.75 = 9
    /// </summary>
    MD,

    /// <summary>
    ///     12 * 1.0 = 12
    /// </summary>
    LG,

    /// <summary>
    ///     12 * 1.5 = 18
    /// </summary>
    XL,

    /// <summary>
    ///     12 * 2.0 = 24
    /// </summary>
    XL2,

    /// <summary>
    ///     12 * 3.0 = 36
    /// </summary>
    XL3,

    /// <summary>
    ///     12 * 4.0 = 48
    /// </summary>
    XL4,

    /// <summary>
    ///     9999
    /// </summary>
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
    public MSizeVariant? Offset { get; init; }
    public MSizeVariant? OffsetNegative { get; init; }

    MRadiusFilter _filter = MRadiusFilter.All;

    public string Filter
    {
        set => _ = value.TryParseEnum(out _filter, '|');
    }

    protected override IEnumerable<object> GetKeys()
    {
        yield return MSizeKey.Radius;
        if (Offset is not null || OffsetNegative is not null)
            yield return MSizeKey.Spacing;
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

        var multiplier = _multipliers[(int)variant];
        double newSize;

        if (multiplier < 0)
        {
            newSize = 9999d;
        }
        else
        {
            newSize = size * multiplier;

            if ((Offset is not null || OffsetNegative is not null) && values[1] is double offsetNumber)
            {
                var offsetSize = Offset.HasValue ? MSizeBase.CalculateSize(offsetNumber, Offset.Value) : 0d;
                var offsetNegativeSize = OffsetNegative.HasValue ? MSizeBase.CalculateSize(offsetNumber, OffsetNegative.Value) : 0d;
                newSize = newSize + offsetSize - offsetNegativeSize;
            }

            if (newSize <= 0d)
                return default(CornerRadius);
        }

        return new CornerRadius(
            _filter.HasFlag(MRadiusFilter.LeftTop, newSize),
            _filter.HasFlag(MRadiusFilter.RightTop, newSize),
            _filter.HasFlag(MRadiusFilter.RightBottom, newSize),
            _filter.HasFlag(MRadiusFilter.LeftBottom, newSize)
        );
    }
}
