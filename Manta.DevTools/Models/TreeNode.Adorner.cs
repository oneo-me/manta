using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Manta.Themes.Variables;

namespace Manta.DevTools.Models;

sealed partial class TreeNode
{
    static IDisposable UpdateAdornerCore(Visual targetControl, IResourceHost resourceHost, bool isDash)
    {
        if (AdornerLayer.GetAdornerLayer(targetControl) is not { } layer)
            return Disposable.Empty;

        if (targetControl is Control control)
            control.BringIntoView(new(-12, -12, targetControl.Bounds.Width + 24, targetControl.Bounds.Height + 24));

        var adorner = new Rectangle
        {
            StrokeThickness = 1,
            [AdornerLayer.AdornedElementProperty] = targetControl,
            [!Shape.FillProperty] = new MBrush(MBrushKey.Primary) { Opacity = 0.03 }.GetResourceBinding(resourceHost),
            [!Shape.StrokeProperty] = new MBrush(MBrushKey.Primary).GetResourceBinding(resourceHost)
        };

        if (isDash)
            adorner.StrokeDashArray = new(5, 3);

        layer.Children.Add(adorner);

        return Disposable.Create((layer, adorner), state =>
        {
            state.layer.Children.Remove(state.adorner);
        });
    }

    IDisposable? _disposable;

    public void UpdateAdorner(IResourceHost resourceHost)
    {
        ClearAdorner();

        _disposable = UpdateAdornerCore(View, resourceHost, true);
    }

    public void ClearAdorner()
    {
        _disposable?.Dispose();
    }
}
