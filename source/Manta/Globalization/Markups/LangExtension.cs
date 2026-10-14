using Avalonia.Data;
using Manta.Globalization.Core;
using Manta.Reactive;

namespace Manta.Globalization.Markups;

public sealed class LangExtension(object langKey) : LangExtensionBase
{
    public object? LangType { get; set; }

    public object LangKey { get; } = langKey;
    public object? DefValue { get; set; }
    public object? Data1 { get; set; }
    public object? Data2 { get; set; }
    public object? Data3 { get; set; }
    public object? Data4 { get; set; }
    public object? Data5 { get; set; }

    protected override void Initialize(MultiBinding multiBinding)
    {
        if (LangType is null)
            multiBinding.Bindings.Add(Bind.Create(Lang.Source, x => x.Current, mode: BindingMode.OneWay));
        else
            BindingValue(multiBinding, LangType, this, x => x.LangType);
        BindingValue(multiBinding, LangKey, this, x => x.LangKey);
        BindingValue(multiBinding, DefValue, this, x => x.DefValue);
        BindingValue(multiBinding, Data1, this, x => x.Data1);
        BindingValue(multiBinding, Data2, this, x => x.Data2);
        BindingValue(multiBinding, Data3, this, x => x.Data3);
        BindingValue(multiBinding, Data4, this, x => x.Data4);
        BindingValue(multiBinding, Data5, this, x => x.Data5);
    }

    protected override object ProcessValues(IList<object?> values)
    {
        if (values[0] is not LangType langType)
            return string.Empty;

        if (values[1] is not string langKey)
            return string.Empty;

        var defValue = values[2] as string;

        var args = values.Count > 3 ? values.Skip(3).ToArray() : null;

        return Lang.Get(new(langKey, args)
        {
            LangType = langType,
            DefValue = defValue
        });
    }
}
