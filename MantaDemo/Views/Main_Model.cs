using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using Manta;
using Manta.Controls;
using Manta.Controls.Icons;
using Manta.Globalization;
using Manta.Reactive.Commands;
using Manta.Reactive.Regions;
using Manta.Reactive.Services;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;
using MantaDemo.Services;

namespace MantaDemo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
    public static Main_Model Design => new();

    readonly ConfigService _configService = Service.GetService<ConfigService>();
    readonly ViewService _viewService = Service.GetService<ViewService>();

    public RegionManager<HeaderRegionModel> RegionManager => _viewService.RegionManager;

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

    public CommandWarper OpenTweakcnToolAsyncCommand => GetCommand(async () =>
    {
        await new TweakcnTool_Model().ShowDialogAsync(this);
    });

    public CommandWarper OpenSettingsAsyncCommand => GetCommand(async () =>
    {
        await new SettingsView_Model().ShowDialogAsync(this);
    });

    public IconSource ThemeIcon
    {
        get;
        set => SetValue(ref field, value);
    } = GetThemeIcon(Application.Current?.RequestedThemeVariant);

    public CommandWarper SwitchThemeCommand => GetCommand(() =>
    {
        ThemeIcon = GetThemeIcon(MApp.SwitchTheme());
    });

    static IconSource GetThemeIcon(ThemeVariant? variant)
    {
        return variant == ThemeVariant.Default ? Icons.ThemeSystem :
            variant == ThemeVariant.Dark ? Icons.ThemeDark : Icons.ThemeLight;
    }
}
