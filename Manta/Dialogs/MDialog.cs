using Avalonia.Controls;
using Manta.Controls;
using Manta.Langs;
using Manta.Reactive.Views;

namespace Manta.Dialogs;

public sealed class MDialogOptions(string message)
{
    public string Title { get; init; } = InnerLangKey.Dialog_Title;
    public string Message { get; init; } = message;

    public string CancelText { get; init; } = InnerLangKey.Dialog_Cancel;
    public string ConfirTextBlock { get; init; } = InnerLangKey.Dialog_Continue;

    public MButtonVariant CancelVariant { get; init; } = MButtonVariant.Secondary;
    public MButtonVariant ConfirmVariant { get; init; } = MButtonVariant.Default;

    public bool ShowCancel { get; internal set; }
    public bool ShowConfirm { get; internal set; }
}

public static class MDialog
{
    static async Task<bool> ShowCore(Window parent, MDialogOptions options)
    {
        var viewModel = new DialogView_Model(options);
        var window = viewModel.CreateWindow();
        await window.ShowDialog(parent);
        return viewModel.Result;
    }

    public static async Task Alert(Window parent, MDialogOptions options)
    {
        options.ShowCancel = false;
        options.ShowConfirm = true;
        await ShowCore(parent, options);
    }

    public static async Task<bool> Confirm(Window parent, MDialogOptions options)
    {
        options.ShowCancel = true;
        options.ShowConfirm = true;
        return await ShowCore(parent, options);
    }
}
