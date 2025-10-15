using Avalonia.Data;
using LightKit.I18N.Core;

namespace LightKit.I18N.Markups;

public sealed class LangNameExtension(object langType) : BindingMarkupExtension
{
    public object LangType { get; } = langType;

    protected override void Initialize(MultiBinding multiBinding)
    {
        BindingValue(multiBinding, LangType, nameof(LangType));
    }

    protected override object ProcessValues(IList<object?> values)
    {
        if (values[0] is not LangType langType)
            return string.Empty;

        var langMetadata = langType.GetMetadata();

        return langMetadata.NativeName;
    }
}
