using System.Globalization;
using System.Linq.Expressions;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Manta.Reactive;

namespace Manta.Globalization.Markups;

public abstract class LangExtensionBase : MarkupExtension, IMultiValueConverter
{
    protected abstract void Initialize(MultiBinding multiBinding);
    protected abstract object ProcessValues(IList<object?> values);

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return ToBinding();
    }

    public BindingBase ToBinding()
    {
        var multiBinding = new MultiBinding
        {
            Mode = BindingMode.OneWay,
            Converter = this
        };

        Initialize(multiBinding);

        return multiBinding;
    }

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return ProcessValues(values);
    }

    protected static void BindingValue<TIn, TOut>(MultiBinding multiBinding, object? value, TIn source, Expression<Func<TIn, TOut>> expression)
    {
        if (value is BindingBase keyBinding)
            multiBinding.Bindings.Add(keyBinding);
        else
            multiBinding.Bindings.Add(Bind.Create(source, expression, mode: BindingMode.OneWay));
    }
}
