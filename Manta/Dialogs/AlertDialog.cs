using Avalonia.Controls;
using Avalonia.Threading;
using Manta.Controls;
using Manta.Langs;
using Manta.Reactive.Views;

namespace Manta.Dialogs;

public sealed class AlertDialogOptions(string title, string content)
{
    public AlertDialogOptions(string content) : this(InnerLangKey.Dialog_Title, content)
    {
        Title = InnerLangKey.Dialog_Title;
        Content = content;
    }

    public string Title { get; init; } = title;
    public string Content { get; init; } = content;
    public string ContinueButton { get; init; } = InnerLangKey.Dialog_Continue;
    public MButtonVariant ContinueButtonVariant { get; init; } = MButtonVariant.Default;
}

public static class AlertDialog
{
    public static async Task ShowDialogAsync(Window parent, AlertDialogOptions options)
    {
        var dialogModel = new DialogView_Model
        {
            Title = options.Title,
            Content = options.Content,
            CancelButton = string.Empty,
            ContinueButton = options.ContinueButton,
            ContinueButtonVariant = options.ContinueButtonVariant
        };

        await Dispatcher.UIThread.InvokeAsync(() => dialogModel.ShowDialog(parent));
    }
}
