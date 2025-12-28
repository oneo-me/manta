using System.Reactive.Disposables;
using Avalonia.Controls;
using Manta.Extensions;
using Manta.Reactive.Extensions;
using Manta.Controls;
using Manta.Reactive.Views;

namespace Manta.Dialogs;

[View<DialogView>]
sealed class DialogView_Model(LightDialogOptions options) : WindowModel
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

    public override void OnWindowInitialized(LightWindow window)
    {
        base.OnWindowInitialized(window);

        window.SizeToContent = SizeToContent.WidthAndHeight;
        window.SystemDecorations = SystemDecorations.BorderOnly;
        window.CanMinimize = false;
        window.CanMaximize = false;
        window.CanResize = false;
        window.ShowInTaskbar = false;
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var view = this.GetView<DialogView>();

        view.SubscribeTitleBarEvents(x => x).DisposeWithIfDisposable(disposables);
    }
}
