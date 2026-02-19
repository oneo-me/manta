using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Manta.DataGrid.Columns;

public class MDataGridTemplateColumn : MDataGridColumn
{
    public IDataTemplate? CellTemplate { get; set; }

    public override Control GenerateCell(object? dataItem)
    {
        if (CellTemplate?.Build(dataItem) is { } content)
            return content;
        return new TextBlock { DataContext = dataItem, Text = dataItem?.ToString() };
    }
}
