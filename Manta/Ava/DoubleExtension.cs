using Avalonia.Markup.Xaml;

namespace Manta.Ava;

public class DoubleExtension(double value) : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return value;
    }
}
