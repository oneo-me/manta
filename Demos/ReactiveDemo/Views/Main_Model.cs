using System.Reactive.Disposables;
using Avalonia.Controls;
using Axe.Reactive;
using ReactiveDemo.Services;

namespace ReactiveDemo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
    readonly AppService _appService = Service.GetService<AppService>();

    public string Folder => DesktopApp.Folder;
    public string File => DesktopApp.File;

    public string TestGetService => _appService.Hello();

    public string Input
    {
        get;
        set => SetValue(ref field, value);
    } = "Empty";

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

        view.SubscribeWindowStatus(x =>
        {
            view.TitleBar.IsShowButtons = x is { isShowButtons: true, isFullscreen: false };
            if (OperatingSystem.IsMacOS())
                view.TitleBar.Padding = new(Design.IsDesignMode || x.isFullscreen ? 0 : x.leftOffset - 8, 0, 0, 0);
            else
                view.TitleBar.Padding = new(5, 0, 0, 0);
        });
    }

    public void OpenNewWindow()
    {
        var mainModel = new Main_Model();
        var window = mainModel.CreateWindow();
        var owner = this.GetWindow();
        window.Width = owner.Width - 50;
        window.Height = owner.Height - 50;
        window.Show(owner);
    }
}
