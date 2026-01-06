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
    S0,
    Px,
    S0_5,
    S1,
    S1_5,
    S2,
    S2_5,
    S3,
    S3_5,
    S4,
    S5,
    S6,
    S7,
    S8,
    S9,
    S10,
    S11,
    S12,
    S14,
    S16,
    S20,
    S24,
    S28,
    S32,
    S36,
    S40,
    S44,
    S48,
    S52,
    S56,
    S60,
    S64,
    S72,
    S80,
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

    protected double CalculateSize(double size, MSizeVariant variant)
    {
        var multiplier = _multipliers[(int)variant];
        var newSize = multiplier < 0 ? 1 : size * multiplier;
        return newSize;
    }
}

public class MSize(MSizeVariant variant) : MSizeBase
{
    protected override object ProcessValues(IList<object?> values)
    {
        var size = 0d;
        if (values[0] is double number)
            size = number;

        return CalculateSize(size, variant);
    }
}
