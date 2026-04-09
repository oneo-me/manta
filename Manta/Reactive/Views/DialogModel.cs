using System.Reactive.Disposables;
using Avalonia.Controls;
using Manta.Controls;
using Manta.Extensions;
using Manta.Reactive.Commands;
using Manta.Reactive.Extensions;

namespace Manta.Reactive.Views;

public abstract class DialogModel : WindowModel
{
    protected virtual bool RegisterTitleBarEvents { get; } = OperatingSystem.IsMacOS() is false;
    public bool Result { get; private set; }

    public CommandWarper CancelAsyncCommand => GetCommand(async () =>
    {
        if (await CanCancelAsync().ConfigureAwait(false) is false)
            return;

        Result = false;
        await CloseAsync().ConfigureAwait(false);
        await OnCancelAsync().ConfigureAwait(false);
    });

    public CommandWarper ContinueAsyncCommand => GetCommand(async () =>
    {
        if (await CanContinueAsync().ConfigureAwait(false) is false)
            return;

        Result = true;
        await CloseAsync().ConfigureAwait(false);
        await OnContinueAsync().ConfigureAwait(false);
    });

    protected virtual Task<bool> CanCancelAsync()
    {
        return Task.FromResult(true);
    }

    protected virtual Task<bool> CanContinueAsync()
    {
        return Task.FromResult(true);
    }

    protected virtual Task OnCancelAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnContinueAsync()
    {
        return Task.CompletedTask;
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        if (RegisterTitleBarEvents is false)
            return;

        var view = this.GetView<MView>();

        view.BindTitleBarEvents()
            .DisposeWith(disposables);
    }

    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);

        window.CanResize = false;
        window.CanMinimize = false;
        window.CanMaximize = false;
        window.ShowInTaskbar = false;
        window.WindowDecorations = WindowDecorations.BorderOnly;
        window.WindowStartupLocation = WindowStartupLocation.Manual;
        window.SizeToContent = SizeToContent.WidthAndHeight;
    }
}
