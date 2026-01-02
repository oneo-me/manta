using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
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
        var value0 = values[0];
        var value1 = MBrush.GetColor(values[1], ColorType);

        if (value0 is not BoxShadows boxShadows || value1 is null)
            return default(BoxShadows);

        var shadows = new List<BoxShadow>();

        foreach (var boxShadow in boxShadows)
            shadows.Add(new()
            {
                OffsetX = boxShadow.OffsetX,
                OffsetY = boxShadow.OffsetY,
                Blur = boxShadow.Blur,
                Spread = boxShadow.Spread,
                IsInset = boxShadow.IsInset,
                Color = value1.ToColor(BrushOpacity > 0 ? BrushOpacity : boxShadow.Color.A / 255d)
            });

        if (shadows.Count == 0)
            return default(BoxShadows);

        if (shadows.Count == 1)
            return new BoxShadows(shadows[0]);

        return new BoxShadows(shadows[0], shadows.Skip(1).ToArray());
    }
}
