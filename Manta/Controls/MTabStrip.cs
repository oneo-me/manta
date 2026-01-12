using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Manta.Controls;

public class MTabStrip : ListBox
{
    public static readonly StyledProperty<MTabVariant> VariantProperty = AvaloniaProperty.Register<MTabStrip, MTabVariant>(
        nameof(Variant));

    public MTabVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
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
    public static readonly StyledProperty<MTabVariant> VariantProperty = AvaloniaProperty.Register<MTabStripItem, MTabVariant>(
        nameof(Variant));

    public MTabVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<MTabStrip>() is { } tabStrip)
            Variant = tabStrip.Variant;
    }
}
