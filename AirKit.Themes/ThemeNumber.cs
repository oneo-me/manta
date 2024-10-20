using System.Diagnostics.CodeAnalysis;
using AirKit.Themes.Base;

namespace AirKit.Themes;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeNumberKey
{
    FontSize
}

public sealed class ThemeNumberKeyExtension(ThemeNumberKey key) : ThemeKeyBase(key);

public sealed class ThemeNumber(ThemeNumberKey key) : ThemeValueBase
{
    protected internal override bool IsDynamic => true;

    protected override IEnumerable<object> GetKeys()
    {
        yield return key;
    }

    protected internal override object OnConvert(IList<object?> values)
    {
        var value0 = values[0];

        return value0 switch
        {
            double d => d,
            int i    => i,
            _        => 0d
        };
    }
}
