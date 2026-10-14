using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;

namespace Manta.Controls;

[TemplatePart(nameof(PART_BorderHost), typeof(Panel))]
[TemplatePart(nameof(PART_Border), typeof(Border))]
public class MTabStrip : TabStrip
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

    Panel? PART_BorderHost;
    Border? PART_Border;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_BorderHost = e.NameScope.Get<Panel>(nameof(PART_BorderHost));
        PART_Border = e.NameScope.Get<Border>(nameof(PART_Border));
    }

    internal void UpdateBorder(MTabStripItem item)
    {
        if (PART_BorderHost is null)
            return;

        if (PART_Border is null)
            return;

        if (item.IsSelected is false)
            return;

        var left = 0d;
        var width = 0d;

        var transform = item.TransformToVisual(PART_BorderHost);
        if (transform.HasValue)
        {
            var point = transform.Value.Transform(new(0, 0));
            left = point.X;
            width = item.Bounds.Width;
        }

        PART_Border.Margin = new(left, 0, 0, 0);
        PART_Border.Width = width;
    }
}

public class MTabStripItem : TabStripItem
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

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsSelectedProperty)
            if (this.FindAncestorOfType<MTabStrip>() is { } tabStrip)
                tabStrip.UpdateBorder(this);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        if (this.FindAncestorOfType<MTabStrip>() is { } tabStrip)
            tabStrip.UpdateBorder(this);
    }
}
