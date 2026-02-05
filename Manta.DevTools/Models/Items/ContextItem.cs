namespace Manta.DevTools.Models.Items;

/// <summary>
/// Represents a ViewContext item with source tracking.
/// </summary>
public sealed class ContextItem
{
    public string Key { get; }
    public bool IsReadOnly { get; }
    public Type SourceViewModel { get; }

    /// <summary>
    /// The source object that owns the context (for binding).
    /// </summary>
    public object Source { get; }

    /// <summary>
    /// The property item for rendering the value editor.
    /// </summary>
    public PropertyItemBase PropertyItem { get; }

    public ContextItem(string key, object? rawValue, Type sourceViewModel, object source)
    {
        Key = key;
        SourceViewModel = sourceViewModel;
        Source = source;

        // Create PropertyItem for rendering - use the key as property name
        // The PropertyItem will handle ReactiveValue unwrapping
        var valueType = rawValue?.GetType() ?? typeof(object);
        var isReactive = rawValue is not null && PropertyItemBase.IsReactiveValueType(valueType);

        PropertyItem = PropertyItemBase.Create(source, key, rawValue, valueType, canWrite: isReactive);
        IsReadOnly = PropertyItem.IsReadOnly;
    }

    public string SourceName => SourceViewModel.Name;
}
