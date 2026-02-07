namespace Manta.NodeEditor.TypeSystem;

public sealed class TypeCompatibility
{
    readonly List<TypeConverter> _converters = [];

    public static TypeCompatibility CreateDefault()
    {
        var tc = new TypeCompatibility();

        // Numeric conversions
        tc.RegisterConverter(typeof(int), typeof(double), v => (double)(int)v!);
        tc.RegisterConverter(typeof(int), typeof(float), v => (float)(int)v!);
        tc.RegisterConverter(typeof(int), typeof(long), v => (long)(int)v!);
        tc.RegisterConverter(typeof(int), typeof(decimal), v => (decimal)(int)v!);
        tc.RegisterConverter(typeof(float), typeof(double), v => (double)(float)v!);
        tc.RegisterConverter(typeof(double), typeof(float), v => (float)(double)v!);
        tc.RegisterConverter(typeof(double), typeof(decimal), v => (decimal)(double)v!);

        // To string
        tc.RegisterConverter(typeof(int), typeof(string), v => v!.ToString()!);
        tc.RegisterConverter(typeof(double), typeof(string), v => v!.ToString()!);
        tc.RegisterConverter(typeof(float), typeof(string), v => v!.ToString()!);
        tc.RegisterConverter(typeof(bool), typeof(string), v => v!.ToString()!);

        return tc;
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
        if (source == target) return true;
        if (target == typeof(object)) return true;
        if (target.IsAssignableFrom(source)) return true;
        return _converters.Any(c => c.SourceType == source && c.TargetType == target);
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
