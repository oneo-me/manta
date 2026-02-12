using System.Collections;
using System.Reflection;
using System.Windows.Input;
using Avalonia.Media;
using Manta.Controls.Icons;
using Manta.Media.Imaging;
using Manta.Reactive.Views;

namespace Manta.DevTools.Models.PropertyItems;

abstract class PropertyItemBase(string name, Type propertyType, object source, string path, bool isReadOnly, Func<object?> getValue) : ItemBase(name, propertyType)
{
    public object Source { get; } = source;
    public string Path { get; } = path;
    public bool IsReadOnly { get; } = isReadOnly;

    public object? GetValue()
    {
        return getValue();
    }

    public static PropertyItemBase Create(object source, PropertyInfo propertyInfo)
    {
        var name = propertyInfo.Name;
        var bindingSource = source;
        var bindingPath = propertyInfo.Name;
        var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
        var isReadOnly = propertyInfo.SetMethod is null;

        return Create(name, bindingSource, bindingPath, propertyType, isReadOnly, () => propertyInfo.GetValue(source));
    }

    public static PropertyItemBase Create(string name, object bindingSource, string bindingPath, Type propertyType, bool isReadOnly, Func<object?> getValue)
    {
        var originalPropertyType = propertyType;
        var getValue1 = () =>
        {
            var obj = getValue();
            if (obj is IReactiveValue reactiveValue)
                return reactiveValue.GetValue();
            return obj;
        };

        if (typeof(IReactiveValue).IsAssignableFrom(originalPropertyType))
        {
            propertyType = propertyType.GenericTypeArguments[0];
            bindingPath = nameof(ReactiveValue<>.Value);
            isReadOnly = false;
        }

        if (typeof(ICommand).IsAssignableFrom(propertyType))
            return new CommandPropertyItem(name, originalPropertyType, bindingSource, bindingPath, isReadOnly, getValue1);

        if (typeof(IconSource).IsAssignableFrom(propertyType))
            return new IconSourcePropertyItem(name, originalPropertyType, bindingSource, bindingPath, isReadOnly, getValue1);

        if (typeof(IImage).IsAssignableFrom(propertyType))
            return new ImagePropertyItem(name, originalPropertyType, bindingSource, bindingPath, isReadOnly, getValue1);

        if (typeof(IImageSource).IsAssignableFrom(propertyType))
            return new ImageSourcePropertyItem(name, originalPropertyType, bindingSource, bindingPath, isReadOnly, getValue1);

        if (propertyType == typeof(bool))
            return new BooleanPropertyItem(name, originalPropertyType, bindingSource, bindingPath, isReadOnly, getValue1);

        if (propertyType == typeof(string) || propertyType == typeof(char) || propertyType == typeof(Guid) || propertyType == typeof(Uri) || propertyType == typeof(TimeSpan) || propertyType == typeof(DateTime) || propertyType == typeof(DateTimeOffset))
            return new StringPropertyItem(name, originalPropertyType, bindingSource, bindingPath, isReadOnly, getValue1);

        if (propertyType == typeof(int) || propertyType == typeof(long) || propertyType == typeof(float) || propertyType == typeof(double) || propertyType == typeof(decimal) || propertyType == typeof(uint) || propertyType == typeof(ulong) || propertyType == typeof(short) || propertyType == typeof(ushort) || propertyType == typeof(byte) || propertyType == typeof(sbyte))
            return new NumericPropertyItem(name, originalPropertyType, bindingSource, bindingPath, isReadOnly, getValue1);

        if (propertyType.IsEnum)
            return new EnumPropertyItem(name, originalPropertyType, bindingSource, bindingPath, isReadOnly, getValue1);

        if (typeof(IEnumerable).IsAssignableFrom(propertyType))
            return new CollectionPropertyItem(name, originalPropertyType, bindingSource, bindingPath, isReadOnly, getValue1);

        return new ObjectPropertyItem(name, originalPropertyType, bindingSource, bindingPath, isReadOnly, getValue1);
    }
}
