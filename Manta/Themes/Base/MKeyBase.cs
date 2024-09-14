using Avalonia.Markup.Xaml;

namespace Manta.Themes.Base;

public abstract class MKeyBase(object key) : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return key;
    }
}
