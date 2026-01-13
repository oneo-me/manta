using System.Globalization;
using System.Linq.Expressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Manta.Reactive;
using Manta.Themes.Base;

namespace Manta.Themes.Variables;

public enum MThemeKey
{
    Variant
}

public sealed class MThemeKeyExtension(MThemeKey key) : MKeyBase(key);

public sealed class OnThemeExtension(object def) : MarkupExtension, IMultiValueConverter
{
    public object? Default { get; } = def;
    public object? Light { get; set; }
    public object? Dark { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var multiBinding = new MultiBinding
        {
            Mode = BindingMode.OneWay,
            Converter = this
        };

        multiBinding.Bindings.Add(new DynamicResourceExtension(MThemeKey.Variant).ProvideValue(serviceProvider));

        BindingValue(multiBinding, Default, this, x => x.Default);
        BindingValue(multiBinding, Light, this, x => x.Light);
        BindingValue(multiBinding, Dark, this, x => x.Dark);

        return multiBinding;
    }

    public BindingBase GetResourceBinding(IResourceHost control)
    {
        var multiBinding = new MultiBinding
        {
            Mode = BindingMode.OneWay,
            Converter = this
        };

        multiBinding.Bindings.Add(control.GetResourceObservable(MThemeKey.Variant).ToBinding());

        BindingValue(multiBinding, Default, this, x => x.Default);
        BindingValue(multiBinding, Light, this, x => x.Light);
        BindingValue(multiBinding, Dark, this, x => x.Dark);

        return multiBinding;
    }

    void BindingValue<TIn, TOut>(MultiBinding multiBinding, object? value, TIn source, Expression<Func<TIn, TOut>> expression)
    {
        if (value is BindingBase keyBinding)
            multiBinding.Bindings.Add(keyBinding);
        else
            multiBinding.Bindings.Add(Bind.Create(source, expression, mode: BindingMode.OneWay));
    }

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return values[0] is not false ? values[2] ?? values[1] : values[3] ?? values[1];
    }
}
