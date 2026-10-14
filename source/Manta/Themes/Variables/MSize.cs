using System.Diagnostics.CodeAnalysis;
using Manta.Themes.Base;

namespace Manta.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MSizeKey
{
    Radius,
    Spacing
}

public sealed class MSizeKeyExtension(MSizeKey sizeKey) : MKeyBase(sizeKey);

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MSizeVariant
{
    /// <summary>
    ///     0
    /// </summary>
    S0,

    /// <summary>
    ///     1
    /// </summary>
    Px,

    /// <summary>
    ///     4 * 0.5 = 1
    /// </summary>
    S0_5,

    /// <summary>
    ///     4 * 1 = 4
    /// </summary>
    S1,

    /// <summary>
    ///     4 * 1.5 = 6
    /// </summary>
    S1_5,

    /// <summary>
    ///     4 * 2 = 8
    /// </summary>
    S2,

    /// <summary>
    ///     4 * 2.5 = 10
    /// </summary>
    S2_5,

    /// <summary>
    ///     4 * 3 = 12
    /// </summary>
    S3,

    /// <summary>
    ///     4 * 3.5 = 14
    /// </summary>
    S3_5,

    /// <summary>
    ///     4 * 4 = 16
    /// </summary>
    S4,

    /// <summary>
    ///     4 * 5 = 20
    /// </summary>
    S5,

    /// <summary>
    ///     4 * 6 = 24
    /// </summary>
    S6,

    /// <summary>
    ///     4 * 7 = 28
    /// </summary>
    S7,

    /// <summary>
    ///     4 * 8 = 32
    /// </summary>
    S8,

    /// <summary>
    ///     4 * 9 = 36
    /// </summary>
    S9,

    /// <summary>
    ///     4 * 10 = 40
    /// </summary>
    S10,

    /// <summary>
    ///     4 * 11 = 44
    /// </summary>
    S11,

    /// <summary>
    ///     4 * 12 = 48
    /// </summary>
    S12,

    /// <summary>
    ///     4 * 14 = 56
    /// </summary>
    S14,

    /// <summary>
    ///     4 * 16 = 64
    /// </summary>
    S16,

    /// <summary>
    ///     4 * 20 = 80
    /// </summary>
    S20,

    /// <summary>
    ///     4 * 24 = 96
    /// </summary>
    S24,

    /// <summary>
    ///     4 * 28 = 112
    /// </summary>
    S28,

    /// <summary>
    ///     4 * 32 = 128
    /// </summary>
    S32,

    /// <summary>
    ///     4 * 36 = 144
    /// </summary>
    S36,

    /// <summary>
    ///     4 * 40 = 160
    /// </summary>
    S40,

    /// <summary>
    ///     4 * 44 = 176
    /// </summary>
    S44,

    /// <summary>
    ///     4 * 48 = 192
    /// </summary>
    S48,

    /// <summary>
    ///     4 * 52 = 208
    /// </summary>
    S52,

    /// <summary>
    ///     4 * 56 = 224
    /// </summary>
    S56,

    /// <summary>
    ///     4 * 60 = 240
    /// </summary>
    S60,

    /// <summary>
    ///     4 * 64 = 256
    /// </summary>
    S64,

    /// <summary>
    ///     4 * 72 = 288
    /// </summary>
    S72,

    /// <summary>
    ///     4 * 80 = 320
    /// </summary>
    S80,

    /// <summary>
    ///     4 * 96 = 384
    /// </summary>
    S96
}

public abstract class MSizeBase : MValueBase
{
    protected override IEnumerable<object> GetKeys()
    {
        yield return MSizeKey.Spacing;
    }

    static readonly double[] _multipliers =
    [
        0, -1, 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 16, 20, 24, 28, 32, 36, 40, 44, 48, 52, 56, 60, 64, 72, 80, 96
    ];

    public static double CalculateSize(double size, MSizeVariant variant)
    {
        var multiplier = _multipliers[(int)variant];
        var newSize = multiplier < 0 ? 1 : size * multiplier;
        return newSize;
    }
}

public class MSize(MSizeVariant variant) : MSizeBase
{
    public MSizeVariant Offset { get; init; } = MSizeVariant.S0;
    public MSizeVariant OffsetNegative { get; init; } = MSizeVariant.S0;

    protected override object ProcessValues(IList<object?> values)
    {
        var size = 0d;
        if (values[0] is double number)
            size = number;

        var offsetSize = CalculateSize(size, Offset);
        var offsetNegativeSize = CalculateSize(size, OffsetNegative);
        var result = CalculateSize(size, variant);
        return result + offsetSize - offsetNegativeSize;
    }
}
