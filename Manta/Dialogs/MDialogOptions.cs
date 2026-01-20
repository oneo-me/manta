using Manta.Controls;
using Manta.Langs;

namespace Manta.Dialogs;

public sealed class MDialogOptions(string content)
{
    public string Header { get; init; } = InnerLangKey.Dialog_Title;
    public string Content { get; init; } = content;

    public string CancelButton { get; init; } = InnerLangKey.Dialog_Cancel;
    public string ContinueButton { get; init; } = InnerLangKey.Dialog_Continue;

    public MButtonVariant CancelButtonVariant { get; init; } = MButtonVariant.Outline;
    public MButtonVariant ContinueButtonVariant { get; init; } = MButtonVariant.Default;
}
