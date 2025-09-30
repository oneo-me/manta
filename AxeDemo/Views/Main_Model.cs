using System.Reactive.Disposables;
using Avalonia.Controls;
using Axe.I18N.Avalonia;
using Axe.Icons;
using Axe.Reactive;
using Axe.Reactive.Regions;
using Axe.Reactive.Services;
using Axe.Reactive.Views;
using AxeDemo.Langs;
using AxeDemo.Reactive;
using AxeDemo.Services;

namespace AxeDemo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
    public static Main_Model Design => new();

    readonly ViewService _viewService = Service.GetService<ViewService>();

    public RegionManager<HeaderRegionModel> RegionManager => _viewService.RegionManager;

    public IconSource ThemeSystemIcon { get; } = Assets.GetIconSource("Icons/Sun-Fog--Streamline-Solar-Ar.svg");
    public IconSource SettingsIcon { get; } = Assets.GetIconSource("Icons/Settings--Streamline-Solar-Ar.svg");

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

    public void OpenSettings()
    {
        var mainModel = new SettingsView_Model();
        var window = mainModel.CreateWindow();
        var owner = this.GetWindow();
        window.ShowDialog(owner);
    }
}
