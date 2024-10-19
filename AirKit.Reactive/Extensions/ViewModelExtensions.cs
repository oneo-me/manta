using System.Reflection;
using AirKit.Reactive.Attributes;
using AirKit.Reactive.Controls;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace AirKit.Reactive.Extensions;

public static class ViewModelExtensions
{
    public static LightView CreateView(this ViewModel viewModel)
    {
        if (viewModel.InnerView is not null)
            return viewModel.InnerView;

        var viewModelType = viewModel.GetType();
        var viewAttribute = viewModelType.GetCustomAttribute<ViewAttribute>();
        if (viewAttribute is null)
            throw new InvalidOperationException($"ViewAttribute not found for {viewModelType}");

        var viewType = viewAttribute.GetViewType();
        var view = Service.CreateInstance<LightView>(viewType);
        viewModel.InnerView = view;
        view.DataContext = viewModel;
        return view;
    }

    public static LightView GetView(this ViewModel viewModel)
    {
        if (viewModel.InnerView is null)
            throw new InvalidOperationException($"View not found for {viewModel}");

        return viewModel.InnerView;
    }

    public static T GetView<T>(this ViewModel viewModel) where T : LightView
    {
        if (viewModel.GetView() is T view)
            return view;

        throw new InvalidOperationException($"View is not of type {typeof(T)}");
    }

    public static Window GetWindow(this ViewModel viewModel)
    {
        var view = viewModel.GetView();
        if (view.GetVisualRoot() is Window window1)
            return window1;

        throw new InvalidOperationException($"Window not found for {viewModel}");
    }
}
