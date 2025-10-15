using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace LightKit.Globalization.Markups;

public abstract class BindingMarkupExtension : MarkupExtension, IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return ProcessValues(values);
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return ToBinding();
    }

    public IBinding ToBinding()
    {
        var multiBinding = new MultiBinding
        {
            Mode = BindingMode.OneWay,
            Converter = this
        };

        Initialize(multiBinding);

        return multiBinding;
    }

    protected void BindingValue(MultiBinding multiBinding, object? value, string valueName = "")
    {
        if (value is BindingBase keyBinding)
            multiBinding.Bindings.Add(keyBinding);
        else
            multiBinding.Bindings.Add(new Binding(valueName)
            {
                Source = this
            });
    }

    protected abstract void Initialize(MultiBinding multiBinding);
    protected abstract object ProcessValues(IList<object?> values);
}
