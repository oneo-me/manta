using Avalonia.Markup.Xaml;

namespace Manta.Themes.Base;

public abstract class MtKeyBase(object key) : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return key;
    }
}
