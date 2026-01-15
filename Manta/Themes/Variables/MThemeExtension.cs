using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Manta.Themes.Base;

namespace Manta.Themes.Variables;

public enum MThemeKey
{
    Variant
}

public sealed class MThemeKeyExtension(MThemeKey key) : MKeyBase(key);

public sealed class MThemeExtension : MarkupExtension, IMultiValueConverter
{
    public object? Default { get; set; }
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

        BindingValue(multiBinding, Default, nameof(Default));
        BindingValue(multiBinding, Light, nameof(Light));
        BindingValue(multiBinding, Dark, nameof(Dark));

        return multiBinding;
    }

    public IBinding GetResourceBinding(IResourceHost control)
    {
        var multiBinding = new MultiBinding
        {
            Mode = BindingMode.OneWay,
            Converter = this
        };

        multiBinding.Bindings.Add(control.GetResourceObservable(MThemeKey.Variant).ToBinding());

        BindingValue(multiBinding, Default, nameof(Default));
        BindingValue(multiBinding, Light, nameof(Light));
        BindingValue(multiBinding, Dark, nameof(Dark));

        return multiBinding;
    }

    void BindingValue(MultiBinding multiBinding, object? value, string valueName = "")
    {
        if (value is IBinding keyBinding)
            multiBinding.Bindings.Add(keyBinding);
        else
            multiBinding.Bindings.Add(new Binding(valueName)
            {
                Source = this
            });
    }

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return values[0] is not false ? values[2] ?? values[1] : values[3] ?? values[1];
    }
}
