using Avalonia.Controls;
using Manta.Controls;
using Manta.Controls.Icons;
using Manta.Demo.Langs;
using Manta.Demo.Reactive;
using Manta.Demo.Services;
using Manta.Globalization;
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
    public IconSource TweakcnToolIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Ruler-Pen--Streamline-Solar-Ar.svg"));

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

    protected override void OnInitialized()
    {
        base.OnInitialized();

        this.SetContext("title", Lang.Get(new(LangKey.Title)));
    }

    public async Task OpenSettingsAsync()
    {
        var owner = this.GetWindow();
        await new SettingsView_Model().ShowDialog(owner);
    }

    public async Task OpenTweakcnToolAsync()
    {
        var owner = this.GetWindow();
        await new TweakcnTool_Model().ShowDialog(owner);
    }
}
