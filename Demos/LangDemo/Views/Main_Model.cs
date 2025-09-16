using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Axe.Reactive;

namespace LangDemo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
    public override void OnWindowInitialized(Window window)
    {
        base.OnWindowInitialized(window);

        window.Width = 800;
        window.Height = 600;

        window.AttachDevTools(new KeyGesture(Key.F12, KeyModifiers.Control));
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var view = this.GetView<Main>();

        view.SubscribeWindowStatus(x =>
        {
            view.TitleBar.IsShowButtons = x is { isShowButtons: true, isFullscreen: false };
            if (OperatingSystem.IsMacOS())
                view.TitleBar.Padding = new(Design.IsDesignMode || x.isFullscreen ? 0 : x.leftOffset - 8, 0, 0, 0);
            else
                view.TitleBar.Padding = new(5, 0, 0, 0);
        }).DisposeWith(disposables);
    }
}
