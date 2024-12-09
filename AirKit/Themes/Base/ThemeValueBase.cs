using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace AirKit.Themes.Base;

public abstract class ThemeValueBase : MarkupExtension, IMultiValueConverter
{
    protected abstract IEnumerable<object> GetKeys();
    protected abstract object OnConvert(IList<object?> values);

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
        return OnConvert(values);
    }
}
