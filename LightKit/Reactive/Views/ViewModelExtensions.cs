using System.Reflection;
using Avalonia.Controls;
using Avalonia.VisualTree;
using LightKit.Controls;
using LightKit.Reactive.Services;

namespace LightKit.Reactive.Views;

public static class ViewModelExtensions
{
    extension(ViewModel viewModel)
    {
        public LightView CreateView()
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

        public LightView GetView()
        {
            if (viewModel.InnerView is null)
                throw new InvalidOperationException($"View not found for {viewModel}");

            return viewModel.InnerView;
        }

        public T GetView<T>() where T : LightView
        {
            if (viewModel.GetView() is T view)
                return view;

            throw new InvalidOperationException($"View is not of type {typeof(T)}");
        }

        public Window GetWindow()
        {
            var view = viewModel.GetView();
            if (view.GetVisualRoot() is Window window1)
                return window1;

            throw new InvalidOperationException($"Window not found for {viewModel}");
        }
    }
}
