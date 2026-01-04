using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Manta.Controls;

public enum MListBoxVertical
{
    Default,
    Sidebar,
    NoStyle
}

public class MListBox : ListBox
{
    public static readonly StyledProperty<MListBoxVertical> VerticalProperty = AvaloniaProperty.Register<MListBox, MListBoxVertical>(
        nameof(Vertical));

    public MListBoxVertical Vertical
    {
        get => GetValue(VerticalProperty);
        set => SetValue(VerticalProperty, value);
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
    public static readonly StyledProperty<MListBoxVertical> VerticalProperty = AvaloniaProperty.Register<MListBoxItem, MListBoxVertical>(
        nameof(Vertical));

    public MListBoxVertical Vertical
    {
        get => GetValue(VerticalProperty);
        set => SetValue(VerticalProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<MListBox>() is { } listBox)
            Vertical = listBox.Vertical;
    }
}
