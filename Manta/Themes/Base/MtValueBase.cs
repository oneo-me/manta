using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace Manta.Themes.Base;

public abstract class MtValueBase : MarkupExtension, IMultiValueConverter
{
    protected abstract IEnumerable<object> GetKeys();
    protected abstract object ProcessValues(IList<object?> values);

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var multiBinding = new MultiBinding
        {
            Mode = BindingMode.OneWay,
            Converter = this
        };

        foreach (var key in GetKeys())
            multiBinding.Bindings.Add(new DynamicResourceExtension(key).ProvideValue(serviceProvider));

        return multiBinding;
    }

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return ProcessValues(values);
    }

    public IBinding GetResourceBinding(IResourceHost control)
    {
        var multiBinding = new MultiBinding
        {
            Mode = BindingMode.OneWay,
            Converter = this
        };

        foreach (var key in GetKeys())
            multiBinding.Bindings.Add(control.GetResourceObservable(key).ToBinding());

        return multiBinding;
    }
}
