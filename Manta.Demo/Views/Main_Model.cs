using System.Reactive.Disposables;
using Avalonia.Controls;
using Manta.Controls;
using Manta.Controls.Icons;
using Manta.Demo.Langs;
using Manta.Demo.Reactive;
using Manta.Demo.Services;
using Manta.Extensions;
using Manta.Globalization;
using Manta.Reactive.Extensions;
using Manta.Reactive.Regions;
using Manta.Reactive.Services;
using Manta.Reactive.Views;

namespace Manta.Demo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
    public static Main_Model Design => new();

    readonly ConfigService _configService = Service.GetService<ConfigService>();
    readonly ViewService _viewService = Service.GetService<ViewService>();

    public RegionManager<HeaderRegionModel> RegionManager => _viewService.RegionManager;

    public IconSource ThemeSystemIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Sun-Fog--Streamline-Solar-Ar.svg"));
    public IconSource SettingsIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Settings--Streamline-Solar-Ar.svg"));

    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);

        window[!Window.TitleProperty] = Lang.GetBinding(new(LangKey.Title));

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

        view.TitleBar
            .BindTitleBarEvents()
            .DisposeWith(disposables);

        view.GetWindowStatusObservable()
            .Subscribe(x =>
            {
                view.TitleBar.Height = 38 + view.TitleBar.BorderThickness.Top + view.TitleBar.BorderThickness.Bottom;
                view.TitleBarButtons.IsVisible = x is { IsShowButtons: true, IsFullScreen: false };
                if (OperatingSystem.IsMacOS())
                    view.TitleBar.Padding = new(IsDesignMode || x.IsFullScreen ? 0 : x.Offset.Left, 0, 0, 0);
            })
            .DisposeWith(disposables);
    }

    public void OpenSettings()
    {
        var mainModel = new SettingsView_Model();
        var window = mainModel.CreateWindow();
        var owner = this.GetWindow();
        window.ShowDialog(owner);
    }
}
