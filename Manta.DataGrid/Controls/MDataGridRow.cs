using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace Manta.DataGrid.Controls;

public class MDataGridRow : TemplatedControl
{
    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<MDataGridRow, bool>(nameof(IsSelected));

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    private MDataGrid? _ownerGrid;
    private Grid? _cellsGrid;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _cellsGrid = e.NameScope.Find<Grid>("PART_CellsGrid");
        RefreshCells();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _ownerGrid = this.FindAncestorOfType<MDataGrid>();
        RefreshCells();
        SyncSelection();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        RefreshCells();
        SyncSelection();
    }

    internal void RefreshCells()
    {
        if (_cellsGrid == null || _ownerGrid == null) return;

        _cellsGrid.Children.Clear();
        _cellsGrid.ColumnDefinitions.Clear();

        for (int i = 0; i < _ownerGrid.Columns.Count; i++)
        {
            var col = _ownerGrid.Columns[i];
            _cellsGrid.ColumnDefinitions.Add(MDataGridHeader.MakeColDef(col));
            var cell = col.GenerateCell(DataContext);
            Grid.SetColumn(cell, i);
            _cellsGrid.Children.Add(cell);
        }
    }

    internal void SyncSelection()
    {
        IsSelected = _ownerGrid != null && Equals(DataContext, _ownerGrid.SelectedItem);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        _ownerGrid?.Select(DataContext);
    }
}
