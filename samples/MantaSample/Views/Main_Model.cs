using Avalonia.Controls;
using Manta;
using Manta.Controls;
using Manta.Globalization;
using Manta.Reactive.Commands;
using Manta.Reactive.Regions;
using Manta.Reactive.Views;
using MantaSample.Langs;
using MantaSample.Reactive;
using MAlert = Manta.Dialogs.MAlert;

namespace MantaSample.Views;

[View<Main>]
sealed partial class Main_Model : WindowModel
{
    public RegionManager<PageModel> RegionManager { get; } = new();

    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);

        window[!Window.TitleProperty] = Lang.GetBinding(new(LangKey.Title));
        window.Width = 880;
        window.Height = 600;
        window.MinWidth = 560;
        window.MinHeight = 420;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        RegionManager.Open(() => new HomePage_Model());
        RegionManager.Open(() => new LocalizationPage_Model());
    }

    public CommandWarper SwitchThemeCommand => GetCommand(() => MApp.SwitchTheme());

    public CommandWarper ShowAboutCommand => GetCommand(async () =>
    {
        await MAlert.ShowDialogAsync(this, new(Lang.Get(new(LangKey.Dialog_About_Content)))).ConfigureAwait(true);
    });
}
