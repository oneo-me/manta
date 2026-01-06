using System.Diagnostics.CodeAnalysis;
using Manta.Themes.Base;

namespace Manta.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MSpacingKey
{
    Spacing
}

public sealed class MSpacingKeyExtension(MSpacingKey key) : MKeyBase(key);

public sealed class MSpacing(MSpacingKey key) : MValueBase
{
    public MSpacingKey Key { get; } = key;
    public MBrushKey ColorKey { get; init; } = MBrushKey.Shadow;
    public MBrushType ColorType { get; set; } = MBrushType.Default;
    public double BrushOpacity { get; init; } = -1;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
        yield return ColorKey;
    }

    protected override object ProcessValues(IList<object?> values)
    {
        return default;
    }
}
