using Axe.Reactive.Dialogs;
using Axe.Reactive.Views;
using AxeDemo.Langs;

namespace AxeDemo.Views;

[View<SettingsView>]
sealed class SettingsView_Model : DialogModel
{
    public static SettingsView_Model Design => new();

    public LangKey Title => LangKey.Settings;
}
