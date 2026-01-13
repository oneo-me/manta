using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Manta.Controls;
using Manta.Reactive.Views;
using Manta.Themes.Variables;

namespace Manta.DevTools.Models;

sealed class ViewNode : IDisposable
{
    public ViewNode? Parent { get; }
    public MView View { get; }
    public ViewModel ViewModel { get; }
    public ViewNodeContainer Container { get; }

    public string Name { get; }

    public ViewNode(ViewNode? parent, MView view, ViewModel viewModel)
    {
        Parent = parent;
        View = view;
        ViewModel = viewModel;
        Container = new(this, view);

        var viewModelType = viewModel.GetType();

        Name = viewModelType.Name;
    }

    public void Dispose()
    {
        Container.Dispose();
    }

    IDisposable? _disposable;

    public void UpdateAdorner()
    {
        ClearAdorner();

        if (AdornerLayer.GetAdornerLayer(View) is { } layer)
        {
            View.BringIntoView(new(-12, -12, View.Bounds.Width + 24, View.Bounds.Height + 24));

            var adorner = new Border
            {
                [AdornerLayer.AdornedElementProperty] = View,
                [!Border.CornerRadiusProperty] = View[!TemplatedControl.CornerRadiusProperty]
            };
            adorner[!Border.BackgroundProperty] = new MBrush(MBrushKey.Desgin) { Opacity = 0.05 }.GetResourceBinding(adorner);
            adorner[!Border.BorderBrushProperty] = new MBrush(MBrushKey.Desgin).GetResourceBinding(adorner);
            adorner[!Border.BorderThicknessProperty] = new MThickness(MSizeVariant.Px).GetResourceBinding(adorner);

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
}
