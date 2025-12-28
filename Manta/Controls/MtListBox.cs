using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Manta.Controls;

public enum MtListBoxType
{
    Normal,
    Sidebar
}

public class MtListBox : ListBox
{
    public static readonly StyledProperty<MtListBoxType> TypeProperty = AvaloniaProperty.Register<MtListBox, MtListBoxType>(
        nameof(Type));

    public MtListBoxType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MtListBoxItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MtListBoxItem>(item, out recycleKey);
    }
}

public class MtListBoxItem : ListBoxItem
{
    public static readonly StyledProperty<MtListBoxType> TypeProperty = AvaloniaProperty.Register<MtListBoxItem, MtListBoxType>(
        nameof(Type));

    public MtListBoxType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<MtListBox>() is { } listBox)
            Type = listBox.Type;
    }
}
