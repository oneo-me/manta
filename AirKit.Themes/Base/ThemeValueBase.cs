using AirKit.Themes.Converters;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace AirKit.Themes.Base;

public abstract class ThemeValueBase : MarkupExtension
{
    protected internal abstract bool IsDynamic { get; }
    protected abstract IEnumerable<object> GetKeys();
    protected internal abstract object OnConvert(IList<object?> values);

    internal IList<object?>? Values { get; private set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var result = new MultiBinding
        {
            Converter = ThemeValueConverter.Instance,
            ConverterParameter = this
        };

        if (IsDynamic)
        {
            foreach (var key in GetKeys())
                result.Bindings.Add(new DynamicResourceExtension(key).ProvideValue(serviceProvider));
        }
        else
        {
            Values ??= new List<object?>();
            Values.Clear();

            foreach (var key in GetKeys())
                Values.Add(new StaticResourceExtension(key).ProvideValue(serviceProvider));
        }

        return result;
    }
}
