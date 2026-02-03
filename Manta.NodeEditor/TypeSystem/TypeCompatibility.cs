using System.Globalization;

namespace Manta.NodeEditor.TypeSystem;

public sealed class TypeCompatibility
{
    readonly List<TypeConverter> _converters = [];

    public void RegisterConverter<TSource, TTarget>(Func<TSource?, TTarget?> converter)
    {
        _converters.Add(new()
        {
            SourceType = typeof(TSource),
            TargetType = typeof(TTarget),
            Convert = value => value is TSource source ? converter(source) : default
        });
    }

    public void RegisterConverter(Type sourceType, Type targetType, TypeConvertFunc converter)
    {
        _converters.Add(new()
        {
            SourceType = sourceType,
            TargetType = targetType,
            Convert = converter
        });
    }

    public bool AreTypesCompatible(Type sourceType, Type targetType)
    {
        // Exact match
        if (sourceType == targetType)
            return true;

        // Target is object (accepts anything)
        if (targetType == typeof(object))
            return true;

        // Covariance: source is derived from target
        if (targetType.IsAssignableFrom(sourceType))
            return true;

        // Check registered converters
        if (_converters.Any(c => c.SourceType == sourceType && c.TargetType == targetType))
            return true;

        // Check if there's an implicit conversion via inheritance chain
        if (_converters.Any(c =>
                c.SourceType.IsAssignableFrom(sourceType) &&
                targetType.IsAssignableFrom(c.TargetType)))
            return true;

        return false;
    }

    public object? ConvertValue(object? value, Type sourceType, Type targetType)
    {
        if (value == null)
            return null;

        // Exact match or assignable
        if (targetType.IsAssignableFrom(sourceType))
            return value;

        // Find and apply converter
        var converter = _converters.FirstOrDefault(c =>
            c.SourceType == sourceType && c.TargetType == targetType);

        if (converter != null)
            return converter.Convert(value);

        // Try to find a compatible converter
        converter = _converters.FirstOrDefault(c =>
            c.SourceType.IsAssignableFrom(sourceType) &&
            targetType.IsAssignableFrom(c.TargetType));

        if (converter != null)
            return converter.Convert(value);

        throw new InvalidOperationException(
            $"Cannot convert from {sourceType.Name} to {targetType.Name}");
    }

    public static TypeCompatibility CreateDefault()
    {
        var compatibility = new TypeCompatibility();

        // Register common numeric conversions
        compatibility.RegisterConverter<int, double>(v => v);
        compatibility.RegisterConverter<int, float>(v => v);
        compatibility.RegisterConverter<int, long>(v => v);
        compatibility.RegisterConverter<int, decimal>(v => v);
        compatibility.RegisterConverter<float, double>(v => v);
        compatibility.RegisterConverter<long, double>(v => v);
        compatibility.RegisterConverter<double, float>(v => (float)v);

        // String conversions
        compatibility.RegisterConverter<int, string>(v => v.ToString());
        compatibility.RegisterConverter<double, string>(v => v.ToString(CultureInfo.InvariantCulture));
        compatibility.RegisterConverter<float, string>(v => v.ToString(CultureInfo.InvariantCulture));
        compatibility.RegisterConverter<bool, string>(v => v.ToString());

        return compatibility;
    }
}
