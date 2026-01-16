using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Manta.Controls;

public enum MListBoxVariant
{
    Default,
    Sidebar,
    NoStyle
}

public class MListBox : ListBox
{
    public static readonly StyledProperty<MListBoxVariant> VariantProperty = AvaloniaProperty.Register<MListBox, MListBoxVariant>(
        nameof(Variant));

    public MListBoxVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
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
    public static readonly StyledProperty<MListBoxVariant> VariantProperty = AvaloniaProperty.Register<MListBoxItem, MListBoxVariant>(
        nameof(Variant));

    public MListBoxVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<MListBox>() is { } listBox)
            Variant = listBox.Variant;
    }
}
