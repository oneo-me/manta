using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Manta.DataGrid.Columns;

namespace Manta.DataGrid.Controls;

public class MDataGridHeader : TemplatedControl
{
    internal MDataGrid? OwnerGrid { get; set; }

    private Grid? _grid;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _grid = e.NameScope.Find<Grid>("PART_Grid");
        RefreshColumnWidths();
    }

    internal void RefreshColumnWidths()
    {
        if (_grid == null || OwnerGrid == null) return;

        _grid.Children.Clear();
        _grid.ColumnDefinitions.Clear();

        for (int i = 0; i < OwnerGrid.Columns.Count; i++)
        {
            var col = OwnerGrid.Columns[i];
            _grid.ColumnDefinitions.Add(MakeColDef(col));
            var header = col.GenerateHeader();
            Grid.SetColumn(header, i);
            _grid.Children.Add(header);
        }
    }

    internal static ColumnDefinition MakeColDef(MDataGridColumn col)
    {
        if (double.IsNaN(col.ActualWidth))
            return new ColumnDefinition(GridLength.Auto);
        return new ColumnDefinition(new GridLength(Math.Max(col.MinWidth, col.ActualWidth)));
    }
}
