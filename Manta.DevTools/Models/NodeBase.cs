using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Manta.Themes.Variables;

namespace Manta.DevTools.Models;

abstract class NodeBase : IDisposable
{
    public string Header { get; protected set; } = string.Empty;

    IDisposable? _disposable;

    protected void UpdateAdornerCore(Visual targetControl, IResourceHost resourceHost, bool isDash)
    {
        ClearAdorner();

        if (AdornerLayer.GetAdornerLayer(targetControl) is { } layer)
        {
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
                adorner.StrokeDashArray = new(4, 2);

            layer.Children.Add(adorner);

            _disposable = Disposable.Create((layer, adorner), state =>
            {
                state.layer.Children.Remove(state.adorner);
            });
        }
    }

    public void ClearAdorner()
    {
        _disposable?.Dispose();
    }

    public void Dispose()
    {
        _disposable?.Dispose();
        DisposeOverride();
    }

    protected abstract void DisposeOverride();
}
