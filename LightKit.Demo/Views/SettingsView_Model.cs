using Avalonia.Controls;
using LightKit.Demo.Langs;
using LightKit.Globalization;
using LightKit.Reactive.Dialogs;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Views;

[View<SettingsView>]
sealed class SettingsView_Model : DialogModel
{
    public static SettingsView_Model Design => new();

    public string Title => LangKey.Settings;

    public override void OnWindowInitialized(Window window)
    {
        base.OnWindowInitialized(window);

        window[!Window.TitleProperty] = Lang.GetBinding(new(Title));
    }
}
