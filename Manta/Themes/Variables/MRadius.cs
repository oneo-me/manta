using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Manta.Themes.Base;

namespace Manta.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MRadiusKey
{
    Radius
}

public enum MRadiusVariant
{
    Default,
    Small,
    Medium,
    Large,
    ExtraLarge
}

public sealed class MRadiusKeyExtension(MRadiusKey key) : MKeyBase(key);

public sealed class MRadius(MRadiusVariant variant) : MValueBase
{
    public MRadiusKey Key { get; set; } = MRadiusKey.Radius;
    public MRadiusVariant Variant { get; } = variant;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }

    protected override object ProcessValues(IList<object?> values)
    {
        var value = values[0];

        if (value is not double number)
            return default(CornerRadius);

        return new CornerRadius(number);
    }
}
