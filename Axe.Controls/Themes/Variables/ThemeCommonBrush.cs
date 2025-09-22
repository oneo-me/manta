using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using Axe.Themes.Base;
using Axe.Themes.Values;

namespace Axe.Controls.Themes.Variables;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ThemeCommonBrushKey
{
    Window_Background,

    Background,
    BorderBrush,
    Foreground,

    Primary,
    Success,
    Warning,
    Error,

    Primary_Foreground,
    Success_Foreground,
    Warning_Foreground,
    Error_Foreground
}

public sealed class ThemeCommonBrushKeyExtension(ThemeCommonBrushKey key) : ThemeKeyBase(key);

public sealed class ThemeCommonBrush(ThemeCommonBrushKey key) : ThemeValueBase
{
    public ThemeCommonBrushKey Key { get; } = key;

    protected override IEnumerable<object> GetKeys()
    {
        yield return Key;
    }

    protected override object OnConvert(IList<object?> values)
    {
        var value = values[0];
        if (value is not ThemeRefValue refValue || refValue.Value is null)
            return Brushes.Transparent;

        return refValue.Value;
    }
}
