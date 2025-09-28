using System.Reactive.Disposables;
using Avalonia.Controls;
using Axe.I18N.Avalonia;
using Axe.Reactive;
using AxeDemo.Demos;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
    public static Main_Model Design => new();

    public RegionManager<HeaderRegionModel> RegionManager { get; } = new();

    public Main_Model()
    {
        RegionManager.Open(() => new OverviewView_Model(RegionManager));
        RegionManager.Open(() => new StylesView_Model(), _ => false);
        RegionManager.Open(() => new ReactiveView_Model(), _ => false);
        RegionManager.Open(() => new MultiLanguageView_Model(), _ => false);
        RegionManager.Open(() => new ControlsView_Model(), _ => false);
    }

    public override void OnWindowInitialized(Window window)
    {
        base.OnWindowInitialized(window);

        window[!Window.TitleProperty] = LangBinding.Get(LangKey.Title);
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
            view.TitleBarButtons.IsVisible = x is { isShowButtons: true, isFullscreen: false };
            if (OperatingSystem.IsMacOS())
                view.TitleBar.Padding = new(Avalonia.Controls.Design.IsDesignMode || x.isFullscreen ? 0 : x.leftOffset, 0, 0, 0);
        });
    }
}
