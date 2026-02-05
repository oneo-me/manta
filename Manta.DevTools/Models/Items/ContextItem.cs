namespace Manta.DevTools.Models.Items;

/// <summary>
/// Represents a ViewContext item with source tracking.
/// </summary>
public sealed class ContextItem
{
    public string Key { get; }
    public object? Value { get; }
    public bool IsReadOnly { get; }
    public Type SourceViewModel { get; }
    public Type ValueType { get; }
    public object? RawValue { get; }
    public bool IsReactive { get; }

    public ContextItem(string key, object? rawValue, Type sourceViewModel)
    {
        Key = key;
        RawValue = rawValue;
        SourceViewModel = sourceViewModel;

        // Check if value is ReactiveValue<T> and unwrap
        if (rawValue is not null && PropertyItemBase.IsReactiveValueType(rawValue.GetType()))
        {
            Value = PropertyItemBase.GetReactiveInnerValue(rawValue);
            ValueType = PropertyItemBase.GetReactiveInnerType(rawValue.GetType()) ?? typeof(object);
            IsReactive = true;
            IsReadOnly = false;
        }
        else
        {
            Value = rawValue;
            ValueType = rawValue?.GetType() ?? typeof(object);
            IsReactive = false;
            IsReadOnly = true;
        }
    }

    public string SourceName => SourceViewModel.Name;
    public string DisplayValue => Value?.ToString() ?? "null";
}
