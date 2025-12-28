using System.Reactive.Disposables;
using Avalonia.Controls;
using Manta.Controls;
using Manta.Controls.Icons;
using Manta.Demo.Langs;
using Manta.Extensions;
using Manta.Globalization;
using Manta.Reactive.Views;

namespace Manta.Demo.Views;

[View<SettingsView>]
sealed class SettingsView_Model : WindowModel
{
    public static SettingsView_Model Design => new();

    public IconSource TestIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Fire--Streamline-Solar-Ar.svg"));

    public override void OnWindowInitialized(MtWindow window)
    {
        base.OnWindowInitialized(window);

        window[!Window.TitleProperty] = Lang.GetBinding(new(LangKey.Settings));
        window.MinWidth = window.Width = 820;
        window.MinHeight = window.Height = 560;
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var view = this.GetView<SettingsView>();

        // Sidebar and TitleBar events
        view.SubscribeTitleBarEvents(x => x.SidebarBackground);
        view.SubscribeTitleBarEvents(x => x.TitleBar);

        view.SubscribeWindowStatus(x =>
        {
            // Sidebar adjustments
            view.Sidebar.Margin = x.topOffset > 0 ? new(0, x.topOffset - view.Sidebar.Padding.Top, 0, 0) : default;

            // TitleBar adjustments
            view.TitleBarButtons.IsVisible = x is { isShowButtons: true, isFullscreen: false };
        });
    }
}
