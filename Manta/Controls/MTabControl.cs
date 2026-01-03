using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Manta.Controls;

public enum MTabControlType
{
    Normal
}

public class MTabControl : TabControl
{
    public static readonly StyledProperty<MTabControlType> TypeProperty = AvaloniaProperty.Register<MTabControl, MTabControlType>(
        nameof(Type));

    public MTabControlType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MTabItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MTabItem>(item, out recycleKey);
    }
}

public class MTabItem : TabItem
{
    public static readonly StyledProperty<MTabControlType> TypeProperty = AvaloniaProperty.Register<MTabItem, MTabControlType>(
        nameof(Type));

    public MTabControlType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<MTabControl>() is { } tabControl)
            Type = tabControl.Type;
    }
}
