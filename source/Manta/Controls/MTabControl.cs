using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;

namespace Manta.Controls;

[TemplatePart(nameof(PART_BorderHost), typeof(Panel))]
[TemplatePart(nameof(PART_Border), typeof(Border))]
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

    Panel? PART_BorderHost;
    Border? PART_Border;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_BorderHost = e.NameScope.Get<Panel>(nameof(PART_BorderHost));
        PART_Border = e.NameScope.Get<Border>(nameof(PART_Border));
    }

    internal void UpdateBorder(MTabItem item)
    {
        if (PART_BorderHost is null)
            return;

        if (PART_Border is null)
            return;

        if (item.IsSelected is false)
            return;

        var top = 0d;
        var left = 0d;
        var width = double.NaN;
        var height = double.NaN;

        var transform = item.TransformToVisual(PART_BorderHost);
        if (transform.HasValue)
        {
            var point = transform.Value.Transform(new(0, 0));
            if (TabStripPlacement is Dock.Left or Dock.Right)
            {
                top = point.Y;
                height = item.Bounds.Height;
            }
            else
            {
                left = point.X;
                width = item.Bounds.Width;
            }
        }

        PART_Border.Margin = new(left, top, 0, 0);
        PART_Border.Width = width;
        PART_Border.Height = height;
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

        if (this.FindAncestorOfType<MTabControl>() is { } tabControl)
            Variant = tabControl.Variant;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsSelectedProperty)
            if (this.FindAncestorOfType<MTabControl>() is { } tabControl)
                tabControl.UpdateBorder(this);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        if (this.FindAncestorOfType<MTabControl>() is { } tabControl)
            tabControl.UpdateBorder(this);
    }
}
