using Avalonia.Markup.Xaml;

namespace Axe.Controls.Themes.Base;

public abstract class ThemeKeyBase(object key) : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return key;
    }
}
