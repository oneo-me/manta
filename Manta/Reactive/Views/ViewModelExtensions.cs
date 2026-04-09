using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;
using Manta.Ava;
using Manta.Controls;

namespace Manta.Reactive.Views;

public static class ViewModelExtensions
{
    internal static MView BuildView(this ViewModel viewModel)
    {
        if (viewModel.InnerView is not null)
            return viewModel.InnerView;

        var view = viewModel.CreateView();
        viewModel.InnerView = view;
        view.DataContext = viewModel;
        return view;
    }

    public static MView GetView(this ViewModel viewModel)
    {
        if (viewModel.InnerView is null)
            throw new InvalidOperationException($"View not found for {viewModel}");

        return viewModel.InnerView;
    }

    public static T GetView<T>(this ViewModel viewModel) where T : MView
    {
        if (viewModel.GetView() is T view)
            return view;

        throw new InvalidOperationException($"View is not of type {typeof(T)}");
    }

    static Window? GetWindowCore(this ViewModel viewModel)
    {
        var view = viewModel.GetView();
        return view.GetVisualRoot() as Window;
    }

    public static async Task<Window> GetWindowAsync(this ViewModel viewModel)
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

    public static async Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(this ViewModel viewModel, FilePickerOpenOptions options)
    {
        var window = await viewModel.GetWindowAsync().ConfigureAwait(false);

        if (UIThread.CheckAccess())
            return await window.StorageProvider.OpenFilePickerAsync(options).ConfigureAwait(false);

        return await UIThread.InvokeAsync(() => window.StorageProvider.OpenFilePickerAsync(options)).ConfigureAwait(false);
    }

    public static async Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(this ViewModel viewModel, FolderPickerOpenOptions options)
    {
        var window = await viewModel.GetWindowAsync().ConfigureAwait(false);

        if (UIThread.CheckAccess())
            return await window.StorageProvider.OpenFolderPickerAsync(options).ConfigureAwait(false);

        return await UIThread.InvokeAsync(() => window.StorageProvider.OpenFolderPickerAsync(options)).ConfigureAwait(false);
    }

    public static async Task<IStorageFile?> SaveFilePickerAsync(this ViewModel viewModel, FilePickerSaveOptions options)
    {
        var window = await viewModel.GetWindowAsync().ConfigureAwait(false);

        if (UIThread.CheckAccess())
            return await window.StorageProvider.SaveFilePickerAsync(options).ConfigureAwait(false);

        return await UIThread.InvokeAsync(() => window.StorageProvider.SaveFilePickerAsync(options)).ConfigureAwait(false);
    }

    public static async Task<IClipboard> GetClipboardAsync(this ViewModel viewModel)
    {
        var window = await viewModel.GetWindowAsync().ConfigureAwait(false);

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
