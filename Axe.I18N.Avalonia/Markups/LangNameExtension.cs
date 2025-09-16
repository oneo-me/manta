using Avalonia.Data;
using Axe.I18N.Core;

namespace Axe.I18N.Avalonia.Markups;

public sealed class LangNameExtension(object langType) : BindingMarkupExtension
{
    public object LangType { get; } = langType;

    protected override void Initialize(MultiBinding multiBinding)
    {
        BindingValue(multiBinding, LangType, nameof(LangType));
    }

    protected override object ProcessValues(IList<object?> values)
    {
        var langType = values[0] is LangType value ? value : throw new ArgumentException("LangType expected");
        var langMetadata = langType.GetMetadata();

        return langMetadata.NativeName;
    }
}
