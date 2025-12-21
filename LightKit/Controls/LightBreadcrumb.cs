using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class LightBreadcrumb : ItemsControl
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new LightBreadcrumbItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<LightBreadcrumbItem>(item, out recycleKey);
    }
}

public enum LightBreadcrumbItemType
{
    Text,
    Arrow,
    Slash
}

public class LightBreadcrumbItem : ContentControl
{
    public static readonly StyledProperty<LightBreadcrumbItemType> TypeProperty = AvaloniaProperty.Register<LightBreadcrumbItem, LightBreadcrumbItemType>(
        nameof(Type));

    public LightBreadcrumbItemType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }
}
