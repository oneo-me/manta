using Avalonia.Data;
using Manta.Globalization.Core;

namespace Manta.Globalization.Markups;

public sealed class LangNameExtension(object langType) : LangExtensionBase
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
