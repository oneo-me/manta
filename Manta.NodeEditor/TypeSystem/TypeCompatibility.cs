namespace Manta.NodeEditor.TypeSystem;

public sealed class TypeCompatibility
{
    readonly List<TypeConverter> _converters = [];

    public static TypeCompatibility CreateDefault()
    {
        return new TypeCompatibility();
    }

    public void RegisterConverter(Type source, Type target, TypeConvertFunc convert)
    {
        _converters.Add(new TypeConverter
        {
            SourceType = source,
            TargetType = target,
            Convert = convert
        });
    }

    public bool AreTypesCompatible(Type source, Type target)
    {
        return source == target;
    }

    public object? ConvertValue(object? value, Type sourceType, Type targetType)
    {
        if (value is null) return null;
        if (sourceType == targetType) return value;
        if (targetType == typeof(object)) return value;
        if (targetType.IsAssignableFrom(sourceType)) return value;

        var converter = _converters.FirstOrDefault(c =>
            c.SourceType == sourceType && c.TargetType == targetType);

        return converter?.Convert(value) ?? value;
    }
}
