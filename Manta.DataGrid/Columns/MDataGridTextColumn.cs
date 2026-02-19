using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;

namespace Manta.DataGrid.Columns;

public class MDataGridTextColumn : MDataGridColumn
{
    public IBinding? Binding { get; set; }

    public override Control GenerateCell(object? dataItem)
    {
        var tb = new TextBlock
        {
            DataContext = dataItem,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Avalonia.Thickness(8, 0),
        };
        if (Binding != null)
            tb.Bind(TextBlock.TextProperty, Binding);
        return tb;
    }
}
