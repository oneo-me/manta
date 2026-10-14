using Avalonia.Markup.Xaml;

namespace Manta.Ava;

public sealed class DoubleExtension(double value) : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return value;
    }
}
