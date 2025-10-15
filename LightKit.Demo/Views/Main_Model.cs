using System.Reactive.Disposables;
using Avalonia.Controls;
using LightKit.Controls.Icons;
using LightKit.Globalization;
using LightKit.Reactive.Regions;
using LightKit.Reactive.Services;
using LightKit.Reactive.Views;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;
using LightKit.Demo.Services;

namespace LightKit.Demo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
    public static Main_Model Design => new();

    readonly ConfigService _configService = Service.GetService<ConfigService>();
    readonly ViewService _viewService = Service.GetService<ViewService>();

    public RegionManager<HeaderRegionModel> RegionManager => _viewService.RegionManager;

    public IconSource ThemeSystemIcon { get; } = Assets.GetIconSource("Icons/Sun-Fog--Streamline-Solar-Ar.svg");
    public IconSource SettingsIcon { get; } = Assets.GetIconSource("Icons/Settings--Streamline-Solar-Ar.svg");

    public override void OnWindowInitialized(Window window)
    {
        base.OnWindowInitialized(window);

        window[!Window.TitleProperty] = LangBinding.Get(LangKey.Title);

        window.MinWidth = 800;
        window.MinHeight = 600;

        window.Width = _configService.Config.Width;
        window.Height = _configService.Config.Height;

        window.Closing += delegate
        {
            _configService.Config.Width = window.Width;
            _configService.Config.Height = window.Height;
        };
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
