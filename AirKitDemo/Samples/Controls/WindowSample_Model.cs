using AirKit;
using AirKit.Attributes;
using AirKit.Controls;
using AirKit.Extensions;
using AirKitDemo.Langs;
using AirKitDemo.Models;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace AirKitDemo.Samples.Controls;

[Sample(SampleGroupType.Controls, LangKey.SampleName_Window)]
[View<WindowSample>]
sealed class WindowSample_Model : ViewModel
{
    [View<View>]
    sealed class CustomWindow_Model(Action<Window> custom, Action<View>? customContent = null) : WindowModel
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            var view = this.GetView();
            customContent?.Invoke(view);
            view.SubscribeTitleBarEvents(x => x);
        }

        public override void OnWindowInitialized(Window window)
        {
            base.OnWindowInitialized(window);
            custom(window);
        }
    }

    public async Task OpenWindowAsync()
    {
        var windowModel = new CustomWindow_Model(x =>
        {
            x.Width = 360;
            x.Height = 300;
        });
        var window = windowModel.CreateWindow();
        var owner = this.GetWindow();
        await window.ShowDialog(owner).ConfigureAwait(false);
    }

    public async Task OpenMiniWindowAsync()
    {
        var windowModel = new CustomWindow_Model(x =>
        {
            x.ExtendClientAreaChromeHints &= ~ ExtendClientAreaChromeHints.OSXThickTitleBar;
            x.Width = 360;
            x.Height = 300;
        });
        var window = windowModel.CreateWindow();
        var owner = this.GetWindow();
        await window.ShowDialog(owner).ConfigureAwait(false);
    }

    public async Task OpenOnlyBorderWindowAsync()
    {
        var windowModel = new CustomWindow_Model(x =>
        {
            x.SystemDecorations = SystemDecorations.BorderOnly;
            x.Width = 360;
            x.Height = 300;
        }, x =>
        {
            var button = new Button
            {
                Classes = { "Button-Primary" },
                Content = "Close Window",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new(12)
            };
            button.Click += (_, _) =>
            {
                if (x.GetVisualRoot() is Window window)
                    window.Close();
            };
            x.Content = button;
        });
        var window = windowModel.CreateWindow();
        var owner = this.GetWindow();
        await window.ShowDialog(owner).ConfigureAwait(false);
    }

    public async Task OpenNativeWindowAsync()
    {
        var windowModel = new CustomWindow_Model(x =>
        {
            x.ExtendClientAreaToDecorationsHint = false;
            x.Width = 360;
            x.Height = 300;
        });
        var window = windowModel.CreateWindow();
        var owner = this.GetWindow();
        await window.ShowDialog(owner).ConfigureAwait(false);
    }
}
