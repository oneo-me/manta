using Avalonia.Data;
using LightKit.Globalization.Core;

namespace LightKit.Globalization.Markups;

public sealed class LangExtension(object langKey) : LangExtensionBase
{
    public object? LangType { get; set; }

    public object LangKey { get; } = langKey;
    public object? DefValue { get; set; } = langKey;
    public object? Data1 { get; set; }
    public object? Data2 { get; set; }
    public object? Data3 { get; set; }
    public object? Data4 { get; set; }
    public object? Data5 { get; set; }

    protected override void Initialize(MultiBinding multiBinding)
    {
        if (LangType is null)
            BindingValue(multiBinding, new Binding(nameof(LangSource.Current), BindingMode.OneWay)
            {
                Source = Lang.Source
            });
        else
            BindingValue(multiBinding, LangType, nameof(LangType));
        BindingValue(multiBinding, LangKey, nameof(LangKey));
        BindingValue(multiBinding, DefValue, nameof(DefValue));
        BindingValue(multiBinding, Data1, nameof(Data1));
        BindingValue(multiBinding, Data2, nameof(Data2));
        BindingValue(multiBinding, Data3, nameof(Data3));
        BindingValue(multiBinding, Data4, nameof(Data4));
        BindingValue(multiBinding, Data5, nameof(Data5));
    }

    protected override object ProcessValues(IList<object?> values)
    {
        if (values[0] is not LangType langType)
            return string.Empty;

        if (values[1] is not string langKey)
            return string.Empty;

        if (values[2] is not string defValue)
            return string.Empty;

        var args = values.Count > 3 ? values.Skip(3).ToArray() : null;

        return Lang.Get(new(langKey)
        {
            LangType = langType,
            DefValue = defValue,
            Args = args
        });
    }
}
