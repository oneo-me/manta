using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;
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

    public static MView GetViewCore(this ViewModel viewModel)
    {
        if (viewModel.InnerView is null)
            throw new InvalidOperationException($"View not found for {viewModel}");

        return viewModel.InnerView;
    }

    public static async Task<TopLevel> GetTopLevelAsync(this ViewModel viewModel)
    {
        var view = viewModel.GetViewCore();

        TopLevel? topLevel;

        if (view.Dispatcher.CheckAccess())
            topLevel = TopLevel.GetTopLevel(view);
        else
            topLevel = await view.Dispatcher.InvokeAsync(() => TopLevel.GetTopLevel(view));

        if (topLevel is null)
            throw new InvalidOperationException("TopLevel not found");

        return topLevel;
    }

    public static async Task<Window> GetWindowAsync(this ViewModel viewModel)
    {
        var topLevel = await viewModel.GetTopLevelAsync().ConfigureAwait(false);

        if (topLevel is not Window window)
            throw new InvalidOperationException("Window not found");

        return window;
    }

    public static async Task<IClipboard> GetClipboardAsync(this ViewModel viewModel)
    {
        var topLevel = await viewModel.GetTopLevelAsync().ConfigureAwait(false);

        IClipboard? clipboard;

        if (topLevel.Dispatcher.CheckAccess())
            clipboard = topLevel.Clipboard;
        else
            clipboard = await topLevel.Dispatcher.InvokeAsync(() => topLevel.Clipboard);

        if (clipboard is null)
            throw new InvalidOperationException("Clipboard not available");

        return clipboard;
    }

    public static async Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(this ViewModel viewModel, FilePickerOpenOptions options)
    {
        var topLevel = await viewModel.GetTopLevelAsync().ConfigureAwait(false);

        if (topLevel.Dispatcher.CheckAccess())
            return await topLevel.StorageProvider.OpenFilePickerAsync(options).ConfigureAwait(false);

        return await topLevel.Dispatcher.InvokeAsync(() => topLevel.StorageProvider.OpenFilePickerAsync(options)).ConfigureAwait(false);
    }

    public static async Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(this ViewModel viewModel, FolderPickerOpenOptions options)
    {
        var topLevel = await viewModel.GetTopLevelAsync().ConfigureAwait(false);

        if (topLevel.Dispatcher.CheckAccess())
            return await topLevel.StorageProvider.OpenFolderPickerAsync(options).ConfigureAwait(false);

        return await topLevel.Dispatcher.InvokeAsync(() => topLevel.StorageProvider.OpenFolderPickerAsync(options)).ConfigureAwait(false);
    }

    public static async Task<IStorageFile?> SaveFilePickerAsync(this ViewModel viewModel, FilePickerSaveOptions options)
    {
        var topLevel = await viewModel.GetTopLevelAsync().ConfigureAwait(false);

        if (topLevel.Dispatcher.CheckAccess())
            return await topLevel.StorageProvider.SaveFilePickerAsync(options).ConfigureAwait(false);

        return await topLevel.Dispatcher.InvokeAsync(() => topLevel.StorageProvider.SaveFilePickerAsync(options)).ConfigureAwait(false);
    }
}
