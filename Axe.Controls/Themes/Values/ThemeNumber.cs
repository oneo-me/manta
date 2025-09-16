using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Axe.Controls.Themes.Base;

namespace Axe.Controls.Themes.Values;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeNumberKey
{
    FontSize
}

public sealed class ThemeNumberKeyExtension(ThemeNumberKey key) : ThemeKeyBase(key);

public sealed class ThemeNumber(ThemeNumberKey key) : ThemeValueBase
{
    public int Offset { get; set; }

    protected override IEnumerable<object> GetKeys()
    {
        yield return key;
    }

    protected override object OnConvert(IList<object?> values)
    {
        var value0 = values[0];

        return value0 switch
        {
            double d => AddOffset(d, Offset),
            int i    => AddOffset(i, Offset),
            _        => 0d
        };
    }

    static T AddOffset<T>(T value, T offset) where T : INumber<T>
    {
        return value + offset;
    }
}
