using Avalonia.Data;
using Axe.I18N.Core;

namespace Axe.I18N.Avalonia.Markups;

public sealed class LangExtension(object langKey) : BindingMarkupExtension
{
    public object? LangType { get; set; }

    public object LangKey { get; } = langKey;
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
        BindingValue(multiBinding, Data1, nameof(Data1));
        BindingValue(multiBinding, Data2, nameof(Data2));
        BindingValue(multiBinding, Data3, nameof(Data3));
        BindingValue(multiBinding, Data4, nameof(Data4));
        BindingValue(multiBinding, Data5, nameof(Data5));
    }

    protected override object ProcessValues(IList<object?> values)
    {
        var langType = values[0] is LangType value ? value : throw new ArgumentException("Culture expected");
        var langKey = values[1] ?? throw new ArgumentException("string expected");
        var args = values.Skip(2).ToArray();

        return Lang.Get(langType, langKey, args);
    }
}
