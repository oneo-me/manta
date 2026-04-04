using System.Reactive.Disposables;
using Avalonia.Controls;
using Manta.Controls;
using Manta.Extensions;
using Manta.Globalization;
using Manta.Reactive.Extensions;
using Manta.Reactive.Views;
using MantaDemo.Langs;

namespace MantaDemo.Views;

[View<SettingsView>]
sealed class SettingsView_Model : WindowModel
{
    public override void OnWindowInitialized(MWindow window)
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
        view.SidebarBackground
            .BindTitleBarEvents()
            .DisposeWith(disposables);
        view.TitleBar
            .BindTitleBarEvents()
            .DisposeWith(disposables);

        view.GetWindowStatusObservable()
            .Subscribe(x =>
            {
                // Sidebar adjustments
                view.Sidebar.Margin = x.Offset.Top > 0 ? new(0, x.Offset.Top - view.Sidebar.Padding.Top, 0, 0) : default;

                // TitleBar adjustments
                view.TitleBarButtons.IsVisible = x is { IsShowButtons: true, IsFullScreen: false };
            })
            .DisposeWith(disposables);
    }
}
