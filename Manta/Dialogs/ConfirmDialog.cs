using Manta.Controls;
using Manta.Langs;
using Manta.Reactive.Views;

namespace Manta.Dialogs;

public sealed class ConfirmDialogOptions(string title, string content)
{
    public ConfirmDialogOptions(string content) : this(InnerLangKey.Dialog_Title, content)
    {
        Title = InnerLangKey.Dialog_Title;
        Content = content;
    }

    public string Title { get; init; } = title;
    public string Content { get; init; } = content;
    public string CancelButton { get; init; } = InnerLangKey.Dialog_Cancel;
    public string ContinueButton { get; init; } = InnerLangKey.Dialog_Continue;
    public MButtonVariant CancelButtonVariant { get; init; } = MButtonVariant.Outline;
    public MButtonVariant ContinueButtonVariant { get; init; } = MButtonVariant.Default;
}

public static class ConfirmDialog
{
    public static async Task<bool> ShowDialogAsync(ViewModel parent, ConfirmDialogOptions options)
    {
        var dialogModel = new DialogView_Model
        {
            Title = options.Title,
            Content = options.Content,
            CancelButton = options.CancelButton,
            CancelButtonVariant = options.CancelButtonVariant,
            ContinueButton = options.ContinueButton,
            ContinueButtonVariant = options.ContinueButtonVariant
        };

        await dialogModel.ShowDialogAsync(parent);

        return dialogModel.Result;
    }
}
