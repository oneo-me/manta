using Avalonia.Markup.Xaml;

namespace AirKit.Themes.Base;

public abstract class ThemeKeyBase(object key) : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return key;
    }
}
