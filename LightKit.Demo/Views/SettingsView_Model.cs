using LightKit.Reactive.Dialogs;
using LightKit.Reactive.Views;
using LightKit.Demo.Langs;

namespace LightKit.Demo.Views;

[View<SettingsView>]
sealed class SettingsView_Model : DialogModel
{
    public static SettingsView_Model Design => new();

    public LangKey Title => LangKey.Settings;
}
