using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Manta.DataGrid.Columns;
using System.Collections.Specialized;

namespace Manta.DataGrid.Controls;

public class MDataGrid : ItemsControl
{
    public static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<MDataGrid, object?>(nameof(SelectedItem));

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public AvaloniaList<MDataGridColumn> Columns { get; } = new();

    private MDataGridHeader? _header;
    private ScrollViewer? _headerScrollViewer;
    private ScrollViewer? _contentScrollViewer;

    static MDataGrid()
    {
        SelectedItemProperty.Changed.AddClassHandler<MDataGrid>((grid, _) => grid.OnSelectedItemChanged());
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
        => new MDataGridRow();

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
        => NeedsContainer<MDataGridRow>(item, out recycleKey);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _header = e.NameScope.Find<MDataGridHeader>("PART_Header");
        _headerScrollViewer = e.NameScope.Find<ScrollViewer>("PART_HeaderScrollViewer");
        _contentScrollViewer = e.NameScope.Find<ScrollViewer>("PART_ContentScrollViewer");

        if (_header != null)
            _header.OwnerGrid = this;

        if (_contentScrollViewer != null)
            _contentScrollViewer.ScrollChanged += OnContentScrollChanged;

        Columns.CollectionChanged += OnColumnsChanged;
        foreach (var col in Columns)
            col.SetOwner(this);

        RecalculateColumnWidths();
    }

    private void OnContentScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_headerScrollViewer != null && _contentScrollViewer != null)
            _headerScrollViewer.Offset = new Vector(_contentScrollViewer.Offset.X, 0);
    }

    private void OnColumnsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (MDataGridColumn col in e.NewItems)
                col.SetOwner(this);
        if (e.OldItems != null)
            foreach (MDataGridColumn col in e.OldItems)
                col.SetOwner(null);
        RecalculateColumnWidths();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == BoundsProperty)
            RecalculateColumnWidths();
    }

    internal void InvalidateColumnWidths() => RecalculateColumnWidths();

    private void RecalculateColumnWidths()
    {
        if (Columns.Count == 0) return;

        double totalWidth = Bounds.Width;
        if (totalWidth <= 0) return;

        double fixedSum = 0;
        double totalStars = 0;
        foreach (var col in Columns)
        {
            if (col.Width.IsAbsolute) fixedSum += col.Width.Value;
            else if (col.Width.IsStar) totalStars += col.Width.Value;
        }

        double remaining = Math.Max(0, totalWidth - fixedSum);

        foreach (var col in Columns)
        {
            if (col.Width.IsAbsolute)
                col.SetActualWidth(col.Width.Value);
            else if (col.Width.IsStar)
                col.SetActualWidth(totalStars > 0 ? (col.Width.Value / totalStars) * remaining : 0);
            else
                col.SetActualWidth(double.NaN);
        }

        _header?.RefreshColumnWidths();
        foreach (var row in GetVisibleRows())
            row.RefreshCells();
    }

    private void OnSelectedItemChanged()
    {
        foreach (var row in GetVisibleRows())
            row.SyncSelection();
    }

    internal void Select(object? item)
    {
        SelectedItem = item;
    }

    private IEnumerable<MDataGridRow> GetVisibleRows()
        => ItemsPanelRoot?.Children.OfType<MDataGridRow>() ?? [];
}
