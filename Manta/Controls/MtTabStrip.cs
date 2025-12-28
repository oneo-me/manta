using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Manta.Controls;

public enum MtTabStripType
{
    Normal,
    RootTab
}

public class MtTabStrip : ListBox
{
    public static readonly StyledProperty<MtTabStripType> TypeProperty = AvaloniaProperty.Register<MtTabStrip, MtTabStripType>(
        nameof(Type));

    public MtTabStripType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MtTabStripItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MtTabStripItem>(item, out recycleKey);
    }
}

public class MtTabStripItem : ListBoxItem
{
    public static readonly StyledProperty<MtTabStripType> TypeProperty = AvaloniaProperty.Register<MtTabStripItem, MtTabStripType>(
        nameof(Type));

    public MtTabStripType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<MtTabStrip>() is { } tabStrip)
            Type = tabStrip.Type;
    }
}
