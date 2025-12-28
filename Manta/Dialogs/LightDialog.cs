using Avalonia.Controls;
using Manta.Reactive.Views;

namespace Manta.Dialogs;

public static class LightDialog
{
    static async Task<bool> ShowCore(Window parent, LightDialogOptions options)
    {
        var viewModel = new DialogView_Model(options);
        var window = viewModel.CreateWindow();
        await window.ShowDialog(parent);
        return viewModel.Result;
    }

    public static async Task Alert(Window parent, LightDialogOptions options)
    {
        options.ShowCancel = false;
        options.ShowConfirm = true;
        await ShowCore(parent, options);
    }

    public static async Task<bool> Confirm(Window parent, LightDialogOptions options)
    {
        options.ShowCancel = true;
        options.ShowConfirm = true;
        return await ShowCore(parent, options);
    }
}
