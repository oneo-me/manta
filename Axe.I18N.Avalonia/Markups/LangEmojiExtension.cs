using Avalonia.Data;
using Axe.I18N.Core;

namespace Axe.I18N.Avalonia.Markups;

public sealed class LangEmojiExtension(object langType) : BindingMarkupExtension
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

        return langMetadata.Emoji;
    }
}
