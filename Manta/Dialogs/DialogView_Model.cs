using System.Reactive.Disposables;
using Manta.Controls;
using Manta.Extensions;
using Manta.Langs;
using Manta.Reactive.Extensions;
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

    public bool Result { get; private set; }

    public void OnContinue()
    {
        Result = true;
        Close();
    }

    public void OnCancel()
    {
        Result = false;
        Close();
    }

    void Close()
    {
        this.GetWindow().Close();
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var view = this.GetView<DialogView>();

        view.BindTitleBarEvents()
            .DisposeWithIfDisposable(disposables);
    }
}
