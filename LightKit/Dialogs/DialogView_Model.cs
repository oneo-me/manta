using System.Reactive.Disposables;
using LightKit.Extensions;
using LightKit.Reactive.Dialogs;
using LightKit.Reactive.Extensions;
using LightKit.Reactive.Views;

namespace LightKit.Dialogs;

[View<DialogView>]
sealed class DialogView_Model(LightDialogOptions options) : DialogModel
{
    public static DialogView_Model Design => new(new("Message...") { ShowCancel = true, ShowConfirm = true });

    public LightDialogOptions Options { get; } = options;
    public bool Result { get; private set; }

    public void OnConfirm()
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

        view.SubscribeTitleBarEvents(x => x).DisposeWithIfDisposable(disposables);
    }
}
