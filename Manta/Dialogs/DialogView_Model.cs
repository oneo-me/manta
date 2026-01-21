using Manta.Controls;
using Manta.Langs;
using Manta.Reactive.Views;

namespace Manta.Dialogs;

[View<DialogView>]
sealed class DialogView_Model : DialogModel
{
    public static DialogView_Model Design => new();

    public string Title { get; init; } = InnerLangKey.Dialog_Title;
    public string Content { get; init; } = InnerLangKey.Dialog_Title;

    public string CancelButton { get; init; } = InnerLangKey.Dialog_Cancel;
    public string ContinueButton { get; init; } = InnerLangKey.Dialog_Continue;

    public MButtonVariant CancelButtonVariant { get; init; } = MButtonVariant.Outline;
    public MButtonVariant ContinueButtonVariant { get; init; } = MButtonVariant.Default;
}
