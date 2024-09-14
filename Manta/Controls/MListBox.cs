using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Manta.Controls;

public enum MListBoxType
{
    Normal,
    Sidebar
}

public class MListBox : ListBox
{
    public static readonly StyledProperty<MListBoxType> TypeProperty = AvaloniaProperty.Register<MListBox, MListBoxType>(
        nameof(Type));

    public MListBoxType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MListBoxItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MListBoxItem>(item, out recycleKey);
    }
}

public class MListBoxItem : ListBoxItem
{
    public static readonly StyledProperty<MListBoxType> TypeProperty = AvaloniaProperty.Register<MListBoxItem, MListBoxType>(
        nameof(Type));

    public MListBoxType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<MListBox>() is { } listBox)
            Type = listBox.Type;
    }
}
