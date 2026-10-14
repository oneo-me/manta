using Manta.Controls;
using Manta.Langs;
using Manta.Reactive.Views;

namespace Manta.Dialogs;

public sealed class AlertOptions(string title, string content)
{
    public AlertOptions(string content) : this(InnerLangKey.Dialog_Title, content)
    {
        Title = InnerLangKey.Dialog_Title;
        Content = content;
    }

    public string Title { get; init; } = title;
    public string Content { get; init; } = content;
    public string ContinueButton { get; init; } = InnerLangKey.Dialog_Continue;
    public MButtonVariant ContinueButtonVariant { get; init; } = MButtonVariant.Default;
}

public static class MAlert
{
    public static async Task ShowDialogAsync(ViewModel parent, AlertOptions options)
    {
        var dialogModel = new DialogView_Model
        {
            Title = options.Title,
            Content = options.Content,
            CancelButton = string.Empty,
            ContinueButton = options.ContinueButton,
            ContinueButtonVariant = options.ContinueButtonVariant
        };

        await dialogModel.ShowDialogAsync(parent).ConfigureAwait(false);
    }
}
