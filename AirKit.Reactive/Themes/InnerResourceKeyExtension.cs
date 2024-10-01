using Avalonia.Markup.Xaml;

namespace AirKit.Reactive.Themes;

sealed class InnerResourceKeyExtension(InnerResourceKey resourceKey) : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return resourceKey;
    }
}
