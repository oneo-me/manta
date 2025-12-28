using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public class MBreadcrumb : ItemsControl
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MBreadcrumbItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MBreadcrumbItem>(item, out recycleKey);
    }
}

public enum MBreadcrumbIteMype
{
    Text,
    Arrow,
    Slash
}

public class MBreadcrumbItem : ContentControl
{
    public static readonly StyledProperty<MBreadcrumbIteMype> TypeProperty = AvaloniaProperty.Register<MBreadcrumbItem, MBreadcrumbIteMype>(
        nameof(Type));

    public MBreadcrumbIteMype Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }
}
