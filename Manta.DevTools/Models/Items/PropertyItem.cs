using System.Collections;
using System.Reflection;
using System.Windows.Input;

namespace Manta.DevTools.Models.Items;

/// <summary>
/// Base class for property items displayed in the property list view.
/// </summary>
public abstract class PropertyItemBase
{
    public string Name { get; }
    public bool IsReadOnly { get; }
    public object Source { get; }
    public Type PropertyType { get; }

    protected PropertyItemBase(string name, bool isReadOnly, object source, Type propertyType)
    {
        Name = name;
        IsReadOnly = isReadOnly;
        Source = source;
        PropertyType = propertyType;
    }

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
public sealed class BooleanPropertyItem : PropertyItemBase
{
    public bool IsReactive { get; }

    public BooleanPropertyItem(string name, bool isReadOnly, object source, Type propertyType, bool isReactive)
        : base(name, isReadOnly, source, propertyType)
    {
        IsReactive = isReactive;
    }
}

/// <summary>
/// String property - displays as text box.
/// </summary>
public sealed class StringPropertyItem : PropertyItemBase
{
    public bool IsReactive { get; }

    public StringPropertyItem(string name, bool isReadOnly, object source, bool isReactive)
        : base(name, isReadOnly, source, typeof(string))
    {
        IsReactive = isReactive;
    }
}

/// <summary>
/// Numeric property - displays as text box with number validation.
/// </summary>
public sealed class NumericPropertyItem : PropertyItemBase
{
    public bool IsReactive { get; }

    public NumericPropertyItem(string name, bool isReadOnly, object source, Type propertyType, bool isReactive)
        : base(name, isReadOnly, source, propertyType)
    {
        IsReactive = isReactive;
    }
}

/// <summary>
/// Enum property - displays as combo box.
/// </summary>
public sealed class EnumPropertyItem : PropertyItemBase
{
    public bool IsReactive { get; }
    public Array EnumValues { get; }

    public EnumPropertyItem(string name, bool isReadOnly, object source, Type propertyType, bool isReactive)
        : base(name, isReadOnly, source, propertyType)
    {
        IsReactive = isReactive;
        EnumValues = Enum.GetValues(propertyType);
    }
}

/// <summary>
/// Command property - displays as execute button.
/// </summary>
public sealed class CommandPropertyItem : PropertyItemBase
{
    public ICommand Command { get; }
    public bool HasParameter { get; }

    public CommandPropertyItem(string name, object source, ICommand command)
        : base(name, isReadOnly: true, source, command.GetType())
    {
        Command = command;
        // Check if command type has generic parameter (e.g., AsyncCommand<T>)
        HasParameter = command.GetType().IsGenericType &&
                       command.GetType().GetGenericArguments().Length > 0 &&
                       command.GetType().GetGenericArguments()[0] != typeof(object);
    }
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
public sealed class ObjectPropertyItem : PropertyItemBase
{
    public object Value { get; }

    public ObjectPropertyItem(string name, object source, Type propertyType, object value)
        : base(name, isReadOnly: true, source, propertyType)
    {
        Value = value;
    }
}

/// <summary>
/// Read-only property - displays value as text.
/// </summary>
public sealed class ReadOnlyPropertyItem : PropertyItemBase
{
    public string DisplayValue { get; }

    public ReadOnlyPropertyItem(string name, object source, Type propertyType, object? value)
        : base(name, isReadOnly: true, source, propertyType)
    {
        DisplayValue = value?.ToString() ?? "null";
    }
}
