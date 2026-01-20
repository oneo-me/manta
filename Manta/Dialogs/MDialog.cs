using Avalonia.Controls;
using Manta.Reactive.Views;

namespace Manta.Dialogs;

public static class MDialog
{
    static async Task<bool> ShowCore(Window parent, MDialogOptions options, bool isAlert)
    {
        var viewModel = new DialogView_Model(options)
        {
            IsShowCancelButton = isAlert is false
        };
        var window = viewModel.CreateWindow();
        await window.ShowDialog(parent);
        return viewModel.Result;
    }

    public static async Task Alert(Window parent, MDialogOptions options)
    {
        await ShowCore(parent, options, true);
    }

    public static async Task<bool> Confirm(Window parent, MDialogOptions options)
    {
        return await ShowCore(parent, options, false);
    }
}
