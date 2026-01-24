using System.Reflection;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using Manta.Controls;
using Manta.Reactive.Services;

namespace Manta.Reactive.Views;

public static class ViewModelExtensions
{
    extension(ViewModel viewModel)
    {
        internal MView CreateView()
        {
            if (viewModel.InnerView is not null)
                return viewModel.InnerView;

            var viewModelType = viewModel.GetType();
            var viewAttribute = viewModelType.GetCustomAttribute<ViewAttribute>();
            if (viewAttribute is null)
                throw new InvalidOperationException($"ViewAttribute not found for {viewModelType}");

            var viewType = viewAttribute.GetViewType();
            var view = Service.CreateInstance<MView>(viewType);
            viewModel.InnerView = view;
            view.DataContext = viewModel;
            return view;
        }

        public MView GetView()
        {
            if (viewModel.InnerView is null)
                throw new InvalidOperationException($"View not found for {viewModel}");

            return viewModel.InnerView;
        }

        public T GetView<T>() where T : MView
        {
            if (viewModel.GetView() is T view)
                return view;

            throw new InvalidOperationException($"View is not of type {typeof(T)}");
        }

        Window? GetWindowCore()
        {
            var view = viewModel.GetView();
            return view.GetVisualRoot() as Window;
        }

        public async Task<Window> GetWindowAsync()
        {
            if (viewModel is WindowModel windowModel)
                return windowModel.GetWindowCore();

            Window? window;

            if (UIThread.CheckAccess())
                window = viewModel.GetWindowCore();
            else
                window = await UIThread.InvokeAsync(viewModel.GetWindowCore);

            if (window is null)
                throw new InvalidOperationException($"Window not found for {viewModel}");

            return window;
        }

        public async Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
        {
            var window = await viewModel.GetWindowAsync();

            if (UIThread.CheckAccess())
                return await window.StorageProvider.OpenFilePickerAsync(options);

            return await UIThread.InvokeAsync(() => window.StorageProvider.OpenFilePickerAsync(options));
        }

        public async Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options)
        {
            var window = await viewModel.GetWindowAsync();

            if (UIThread.CheckAccess())
                return await window.StorageProvider.OpenFolderPickerAsync(options);

            return await UIThread.InvokeAsync(() => window.StorageProvider.OpenFolderPickerAsync(options));
        }

        public async Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options)
        {
            var window = await viewModel.GetWindowAsync();

            if (UIThread.CheckAccess())
                return await window.StorageProvider.SaveFilePickerAsync(options);

            return await UIThread.InvokeAsync(() => window.StorageProvider.SaveFilePickerAsync(options));
        }

        public async Task<IClipboard> GetClipboardAsync()
        {
            var window = await viewModel.GetWindowAsync();

            IClipboard? clipboard;

            if (UIThread.CheckAccess())
                clipboard = window.Clipboard;
            else
                clipboard = await UIThread.InvokeAsync(() => window.Clipboard);

            if (clipboard is null)
                throw new InvalidOperationException("Clipboard not available");

            return clipboard;
        }
    }
}
