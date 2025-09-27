using System.Diagnostics;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Axe.Reactive;

namespace AxeDemo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
    public override void OnWindowInitialized(Window window)
    {
        base.OnWindowInitialized(window);

        window.Width = 800;
        window.Height = 600;
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var view = this.GetView<Main>();
        view.SubscribeTitleBarEvents(x => x.TitleBar);
        view.SubscribeWindowStatus(x =>
        {
            view.TitleBar.Height = 38 + view.TitleBar.BorderThickness.Top + view.TitleBar.BorderThickness.Bottom;
            view.TitleBarButtons.IsVisible = Debugger.IsAttached || x is { isShowButtons: true, isFullscreen: false };
            if (OperatingSystem.IsMacOS())
                view.TitleBar.Padding = new(Design.IsDesignMode || x.isFullscreen ? 0 : x.leftOffset - 8, 0, 0, 0);
            else
                view.TitleBar.Padding = new(5, 0, 0, 0);
        });
    }
}
