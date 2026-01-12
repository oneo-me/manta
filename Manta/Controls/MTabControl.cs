using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Manta.Controls;

public class MTabControl : TabControl
{
    public static readonly StyledProperty<MTabVariant> VariantProperty = AvaloniaProperty.Register<MTabControl, MTabVariant>(
        nameof(Variant));

    public MTabVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
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
    public static readonly StyledProperty<MTabVariant> VariantProperty = AvaloniaProperty.Register<MTabItem, MTabVariant>(
        nameof(Variant));

    public MTabVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<MTabControl>() is { } tabStrip)
            Variant = tabStrip.Variant;
    }
}
