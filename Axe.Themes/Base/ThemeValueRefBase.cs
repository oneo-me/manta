using System.Globalization;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Axe.Themes.Variables;

namespace Axe.Themes.Base;

public abstract class ThemeValueRefBase : MarkupExtension, IMultiValueConverter
{
    protected abstract IEnumerable<object> GetKeys();

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var multiBinding = new MultiBinding
        {
            Mode = BindingMode.OneWay,
            Converter = this,
            ConverterParameter = serviceProvider
        };

        foreach (var key in GetKeys())
            multiBinding.Bindings.Add(new DynamicResourceExtension(key).ProvideValue(serviceProvider));

        return multiBinding;
    }

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var serviceProvider = (IServiceProvider)parameter!;

        var value = values[0];
        if (value is not ThemeRef { Value: MultiBinding multiBinding })
            return AvaloniaProperty.UnsetValue;

        if (multiBinding.Bindings[0] is not DynamicResourceExtension dynamicResourceExtension)
            return AvaloniaProperty.UnsetValue;

        var provideValue = dynamicResourceExtension.ProvideValue(serviceProvider);
        return provideValue;
    }
}
