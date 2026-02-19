using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Manta.DataGrid.Columns;

namespace Manta.DataGrid.Controls;

public class MDataGridColumnHeader : ContentControl
{
    internal MDataGridColumn? Column { get; set; }

    private double _dragStartX;
    private double _dragStartWidth;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var grip = e.NameScope.Find<Rectangle>("PART_ResizeGrip");
        if (grip != null)
        {
            grip.PointerPressed += OnGripPointerPressed;
            grip.PointerMoved += OnGripPointerMoved;
            grip.PointerReleased += OnGripPointerReleased;
        }
    }

    private void OnGripPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (Column == null || sender is not Rectangle grip) return;
        _dragStartX = e.GetPosition(null).X;
        _dragStartWidth = Column.Width.IsAbsolute ? Column.Width.Value : Column.ActualWidth;
        if (double.IsNaN(_dragStartWidth) || _dragStartWidth <= 0)
            _dragStartWidth = Bounds.Width;
        e.Pointer.Capture(grip);
        e.Handled = true;
    }

    private void OnGripPointerMoved(object? sender, PointerEventArgs e)
    {
        if (Column == null || !e.GetCurrentPoint(null).Properties.IsLeftButtonPressed) return;
        var delta = e.GetPosition(null).X - _dragStartX;
        var newWidth = Math.Max(Column.MinWidth, _dragStartWidth + delta);
        Column.Width = new GridLength(newWidth);
        e.Handled = true;
    }

    private void OnGripPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        e.Pointer.Capture(null);
    }
}
