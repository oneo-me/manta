using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Manta.Controls;

public enum MTabStripType
{
    Normal
}

public class MTabStrip : ListBox
{
    public static readonly StyledProperty<MTabStripType> TypeProperty = AvaloniaProperty.Register<MTabStrip, MTabStripType>(
        nameof(Type));

    public MTabStripType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MTabStripItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MTabStripItem>(item, out recycleKey);
    }
}

public class MTabStripItem : ListBoxItem
{
    public static readonly StyledProperty<MTabStripType> TypeProperty = AvaloniaProperty.Register<MTabStripItem, MTabStripType>(
        nameof(Type));

    public MTabStripType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<MTabStrip>() is { } tabStrip)
            Type = tabStrip.Type;
    }
}
