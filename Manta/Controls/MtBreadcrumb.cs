using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public class MtBreadcrumb : ItemsControl
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MtBreadcrumbItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MtBreadcrumbItem>(item, out recycleKey);
    }
}

public enum MtBreadcrumbItemType
{
    Text,
    Arrow,
    Slash
}

public class MtBreadcrumbItem : ContentControl
{
    public static readonly StyledProperty<MtBreadcrumbItemType> TypeProperty = AvaloniaProperty.Register<MtBreadcrumbItem, MtBreadcrumbItemType>(
        nameof(Type));

    public MtBreadcrumbItemType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }
}
