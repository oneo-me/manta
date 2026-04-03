using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

namespace Manta.Controls;

[TemplatePart(nameof(PART_BorderHost), typeof(Panel))]
[TemplatePart(nameof(PART_Border), typeof(Border))]
[TemplatePart("PART_RightContentPresenter", typeof(ContentPresenter))]
public class MTabTitleBar : ListBox
{
    static MTabTitleBar()
    {
        RightContentProperty.Changed.AddClassHandler<MTabTitleBar>((x, e) => x.RightContentChanged(e));
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MTabTitleBarItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MTabTitleBarItem>(item, out recycleKey);
    }

    public static readonly StyledProperty<object?> RightContentProperty = AvaloniaProperty.Register<MTabTitleBar, object?>(
        nameof(RightContent));

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> RightContentTemplateProperty = AvaloniaProperty.Register<MTabTitleBar, IDataTemplate?>(
        nameof(RightContentTemplate));

    public IDataTemplate? RightContentTemplate
    {
        get => GetValue(RightContentTemplateProperty);
        set => SetValue(RightContentTemplateProperty, value);
    }

    Panel? PART_BorderHost;
    Border? PART_Border;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_BorderHost = e.NameScope.Get<Panel>(nameof(PART_BorderHost));
        PART_Border = e.NameScope.Get<Border>(nameof(PART_Border));
    }

    void RightContentChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ILogical oldValue)
            LogicalChildren.Remove(oldValue);

        if (e.NewValue is not ILogical newValue)
            return;

        LogicalChildren.Add(newValue);
    }

    internal void UpdateBorder(MTabTitleBarItem item)
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

public class MTabTitleBarItem : ListBoxItem
{
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsSelectedProperty)
            if (this.FindAncestorOfType<MTabTitleBar>() is { } tabTitleBar)
                tabTitleBar.UpdateBorder(this);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        if (this.FindAncestorOfType<MTabTitleBar>() is { } tabTitleBar)
            tabTitleBar.UpdateBorder(this);
    }
}
