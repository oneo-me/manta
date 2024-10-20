using System.Diagnostics.CodeAnalysis;
using AirKit.Themes.Base;
using Avalonia;

namespace AirKit.Themes;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeRadiusKey
{
    Normal,
    Flyout,
    Dialog
}

public sealed class ThemeRadiusKeyExtension(ThemeRadiusKey key) : ThemeKeyBase(key);

public sealed class ThemeRadius(ThemeRadiusKey key) : ThemeValueBase
{
    protected internal override bool IsDynamic => true;

    protected override IEnumerable<object> GetKeys()
    {
        yield return key;
    }

    protected internal override object OnConvert(IList<object?> values)
    {
        var value0 = values[0];

        if (value0 is double value)
            return new CornerRadius(value);
        if (value0 is CornerRadius cornerRadius)
            return cornerRadius;

        return default(CornerRadius);
    }
}
