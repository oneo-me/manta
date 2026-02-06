using System.Collections;
using System.Reflection;
using System.Windows.Input;

namespace Manta.DevTools.Models.Items;

/// <summary>
/// Base class for property items displayed in the property list view.
/// </summary>
public abstract class PropertyItemBase(string name, bool isReadOnly, object source, Type propertyType)
{
    public string Name { get; } = name;
    public bool IsReadOnly { get; } = isReadOnly;
    public object Source { get; } = source;
    public Type PropertyType { get; } = propertyType;

    /// <summary>
    /// Creates a PropertyItem from a PropertyInfo.
    /// Handles ReactiveValue unwrapping automatically.
    /// </summary>
    public static PropertyItemBase Create(object source, PropertyInfo propertyInfo)
    {
        var name = propertyInfo.Name;
        var value = propertyInfo.GetValue(source);
        var propertyType = propertyInfo.PropertyType;
        var canWrite = propertyInfo.SetMethod is not null;

        return Create(source, name, value, propertyType, canWrite);
    }

    /// <summary>
    /// Creates a PropertyItem from raw values.
    /// </summary>
    public static PropertyItemBase Create(object source, string name, object? value, Type propertyType, bool canWrite)
    {
        // Handle ReactiveValue<T> - unwrap and use inner type
        if (IsReactiveValueType(propertyType))
        {
            var innerType = GetReactiveInnerType(propertyType);
            var innerValue = GetReactiveInnerValue(value);
            // ReactiveValue is always writable through .Value property
            return CreateForType(source, name, innerValue, innerType ?? typeof(object), isReadOnly: false, isReactive: true);
        }

        return CreateForType(source, name, value, propertyType, isReadOnly: !canWrite, isReactive: false);
    }

    static PropertyItemBase CreateForType(object source, string name, object? value, Type valueType, bool isReadOnly, bool isReactive)
    {
        // Command - special handling
        if (value is ICommand command)
            return new CommandPropertyItem(name, source, command);

        // Boolean
        if (valueType == typeof(bool) || valueType == typeof(bool?))
            return new BooleanPropertyItem(name, isReadOnly, source, valueType, isReactive);

        // String
        if (valueType == typeof(string))
            return new StringPropertyItem(name, isReadOnly, source, isReactive);

        // Numeric types
        if (IsNumericType(valueType))
            return new NumericPropertyItem(name, isReadOnly, source, valueType, isReactive);

        // Enum
        if (valueType.IsEnum)
            return new EnumPropertyItem(name, isReadOnly, source, valueType, isReactive);

        // Collection (but not string)
        if (value is IEnumerable && valueType != typeof(string))
            return new CollectionPropertyItem(name, source, valueType, value);

        // Complex object (non-null, non-simple type)
        if (value is not null && !IsSimpleType(valueType))
            return new ObjectPropertyItem(name, source, valueType, value);

        // Default: read-only display
        return new ReadOnlyPropertyItem(name, source, valueType, value);
    }

    static bool IsNumericType(Type type)
    {
        var t = Nullable.GetUnderlyingType(type) ?? type;
        return t == typeof(int) || t == typeof(long) || t == typeof(short) || t == typeof(byte) ||
               t == typeof(sbyte) || t == typeof(uint) || t == typeof(ulong) || t == typeof(ushort) ||
               t == typeof(float) || t == typeof(double) || t == typeof(decimal);
    }

    static bool IsSimpleType(Type type)
    {
        var t = Nullable.GetUnderlyingType(type) ?? type;
        return t.IsPrimitive ||
               t == typeof(string) ||
               t == typeof(decimal) ||
               t == typeof(DateTime) ||
               t == typeof(DateTimeOffset) ||
               t == typeof(TimeSpan) ||
               t == typeof(Guid) ||
               t.IsEnum;
    }

    public static bool IsReactiveValueType(Type type)
    {
        if (!type.IsGenericType)
            return false;

        var genericDef = type.GetGenericTypeDefinition();
        // Check by name pattern since we can't reference ReactiveValue<> directly (circular dependency)
        return genericDef.FullName?.StartsWith("Manta.Reactive.Views.ReactiveValue`") == true ||
               genericDef.Name == "ReactiveValue`1";
    }

    public static Type? GetReactiveInnerType(Type type)
    {
        if (!IsReactiveValueType(type))
            return null;
        return type.GetGenericArguments()[0];
    }

    public static object? GetReactiveInnerValue(object? value)
    {
        if (value is null)
            return null;
        var type = value.GetType();
        if (!IsReactiveValueType(type))
            return null;
        var valueProp = type.GetProperty("Value");
        return valueProp?.GetValue(value);
    }
}

/// <summary>
/// Boolean property - displays as checkbox.
/// </summary>
public sealed class BooleanPropertyItem(string name, bool isReadOnly, object source, Type propertyType, bool isReactive)
    : PropertyItemBase(name, isReadOnly, source, propertyType)
{
    public bool IsReactive { get; } = isReactive;
}

/// <summary>
/// String property - displays as text box.
/// </summary>
public sealed class StringPropertyItem(string name, bool isReadOnly, object source, bool isReactive)
    : PropertyItemBase(name, isReadOnly, source, typeof(string))
{
    public bool IsReactive { get; } = isReactive;
}

/// <summary>
/// Numeric property - displays as text box with number validation.
/// </summary>
public sealed class NumericPropertyItem(string name, bool isReadOnly, object source, Type propertyType, bool isReactive)
    : PropertyItemBase(name, isReadOnly, source, propertyType)
{
    public bool IsReactive { get; } = isReactive;
}

/// <summary>
/// Enum property - displays as combo box.
/// </summary>
public sealed class EnumPropertyItem(string name, bool isReadOnly, object source, Type propertyType, bool isReactive)
    : PropertyItemBase(name, isReadOnly, source, propertyType)
{
    public bool IsReactive { get; } = isReactive;
    public Array EnumValues { get; } = Enum.GetValues(propertyType);
}

/// <summary>
/// Command property - displays as execute button.
/// </summary>
public sealed class CommandPropertyItem(string name, object source, ICommand command) : PropertyItemBase(name, isReadOnly: true, source, command.GetType())
{
    public ICommand Command { get; } = command;
    public bool HasParameter { get; } = command.GetType().IsGenericType &&
                                        command.GetType().GetGenericArguments().Length > 0 &&
                                        command.GetType().GetGenericArguments()[0] != typeof(object);

    // Check if command type has generic parameter (e.g., AsyncCommand<T>)
}

/// <summary>
/// Collection property - displays as button that opens collection dialog.
/// </summary>
public sealed class CollectionPropertyItem : PropertyItemBase
{
    public int Count { get; }
    public object? Value { get; }

    public CollectionPropertyItem(string name, object source, Type propertyType, object? value)
        : base(name, isReadOnly: true, source, propertyType)
    {
        Value = value;
        Count = value switch
        {
            ICollection collection => collection.Count,
            IEnumerable enumerable => enumerable.Cast<object>().Count(),
            _ => 0
        };
    }
}

/// <summary>
/// Complex object property - displays as button that opens object dialog.
/// </summary>
public sealed class ObjectPropertyItem(string name, object source, Type propertyType, object value)
    : PropertyItemBase(name, isReadOnly: true, source, propertyType)
{
    public object Value { get; } = value;
}

/// <summary>
/// Read-only property - displays value as text.
/// </summary>
public sealed class ReadOnlyPropertyItem(string name, object source, Type propertyType, object? value)
    : PropertyItemBase(name, isReadOnly: true, source, propertyType)
{
    public string DisplayValue { get; } = value?.ToString() ?? "null";
}
