using System.Reflection;
using Avalonia.Controls;
using Avalonia.VisualTree;
using LightKit.Controls;
using LightKit.Reactive.Services;

namespace LightKit.Reactive.Views;

public static class ViewModelExtensions
{
    public static View CreateView(this ViewModel viewModel)
    {
        if (viewModel.InnerView is not null)
            return viewModel.InnerView;

        var viewModelType = viewModel.GetType();
        var viewAttribute = viewModelType.GetCustomAttribute<ViewAttribute>();
        if (viewAttribute is null)
            throw new InvalidOperationException($"ViewAttribute not found for {viewModelType}");

        var viewType = viewAttribute.GetViewType();
        var view = Service.CreateInstance<View>(viewType);
        viewModel.InnerView = view;
        view.DataContext = viewModel;
        return view;
    }

    public static View GetView(this ViewModel viewModel)
    {
        if (viewModel.InnerView is null)
            throw new InvalidOperationException($"View not found for {viewModel}");

        return viewModel.InnerView;
    }

    public static T GetView<T>(this ViewModel viewModel) where T : View
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
