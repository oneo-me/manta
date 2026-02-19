using Avalonia;
using Avalonia.Controls;
using Manta.DataGrid.Controls;

namespace Manta.DataGrid.Columns;

public abstract class MDataGridColumn : AvaloniaObject
{
    public static readonly StyledProperty<object?> HeaderProperty =
        AvaloniaProperty.Register<MDataGridColumn, object?>(nameof(Header));

    public static readonly StyledProperty<GridLength> WidthProperty =
        AvaloniaProperty.Register<MDataGridColumn, GridLength>(nameof(Width), GridLength.Auto);

    public static readonly StyledProperty<double> MinWidthProperty =
        AvaloniaProperty.Register<MDataGridColumn, double>(nameof(MinWidth), 60.0);

    internal static readonly StyledProperty<double> ActualWidthProperty =
        AvaloniaProperty.Register<MDataGridColumn, double>(nameof(ActualWidth), double.NaN);

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public GridLength Width
    {
        get => GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    public double MinWidth
    {
        get => GetValue(MinWidthProperty);
        set => SetValue(MinWidthProperty, value);
    }

    internal double ActualWidth => GetValue(ActualWidthProperty);

    internal void SetActualWidth(double value) => SetValue(ActualWidthProperty, value);

    private MDataGrid? _ownerGrid;

    internal void SetOwner(MDataGrid? grid) => _ownerGrid = grid;

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == WidthProperty)
            _ownerGrid?.InvalidateColumnWidths();
    }

    public abstract Control GenerateCell(object? dataItem);

    public virtual Control GenerateHeader()
    {
        return new MDataGridColumnHeader { Column = this, Content = Header };
    }
}
