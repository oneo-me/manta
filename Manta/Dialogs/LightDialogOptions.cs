using Manta.Langs;
using Manta.Themes.Variables;

namespace Manta.Dialogs;

public class LightDialogOptions(string message)
{
    public string Title { get; init; } = InnerLangKey.Dialog_Title;
    public string Message { get; init; } = message;

    public string CancelText { get; init; } = InnerLangKey.Dialog_Cancel;
    public string ConfirmText { get; init; } = InnerLangKey.Dialog_Confirm;

    public ColorScheme CancelColor { get; init; } = ColorScheme.Normal;
    public ColorScheme ConfirmColor { get; init; } = ColorScheme.Primary;

    public bool ShowCancel { get; internal set; }
    public bool ShowConfirm { get; internal set; }
}
