using System.Reactive.Disposables;
using Manta.Controls;
using Manta.Extensions;
using Manta.Langs;
using Manta.Reactive.Extensions;
using Manta.Reactive.Views;

namespace Manta.Dialogs;

[View<DialogView>]
sealed partial class DialogView_Model : DialogModel
{
    public string Title { get; init; } = InnerLangKey.Dialog_Title;
    public string Content { get; init; } = InnerLangKey.Dialog_Title;

    public string CancelButton { get; init; } = InnerLangKey.Dialog_Cancel;
    public string ContinueButton { get; init; } = InnerLangKey.Dialog_Continue;

    public MButtonVariant CancelButtonVariant { get; init; } = MButtonVariant.Outline;
    public MButtonVariant ContinueButtonVariant { get; init; } = MButtonVariant.Default;

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var dialogView = this.GetView<DialogView>();

        dialogView.BindTitleBarEvents()
            .DisposeWith(disposables);
    }
}
