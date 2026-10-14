using Manta.Globalization;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;
using MantaSample.Langs;
using MantaSample.Reactive;
using MAlert = Manta.Dialogs.MAlert;
using MConfirm = Manta.Dialogs.MConfirm;

namespace MantaSample.Views;

[View<HomeView>]
sealed partial class HomePage_Model : PageModel
{
    public override string Header => LangKey.Page_Home;

    public string Name
    {
        get;
        set => SetField(ref field, value);
    } = string.Empty;

    public int Count
    {
        get;
        set => SetField(ref field, value);
    }

    public bool Notifications
    {
        get;
        set => SetField(ref field, value);
    } = true;

    public double Volume
    {
        get;
        set => SetField(ref field, value);
    } = 50;

    public string[] Controls { get; } = ["Button", "CheckBox", "ComboBox", "Slider"];

    public string SelectedControl
    {
        get;
        set => SetField(ref field, value);
    } = "Button";

    public string ConfirmResult
    {
        get;
        set => SetField(ref field, value);
    } = string.Empty;

    public CommandWarper IncrementCommand => GetCommand(() => Count++);

    public CommandWarper ResetCommand => GetCommand(() =>
    {
        Count = 0;
        Name = string.Empty;
        ConfirmResult = string.Empty;
    });

    public CommandWarper ShowAlertCommand => GetCommand(async () =>
    {
        await MAlert.ShowDialogAsync(this, new(Lang.Get(new(LangKey.Basics_Alert_Content)))).ConfigureAwait(true);
    });

    public CommandWarper ShowConfirmCommand => GetCommand(async () =>
    {
        var result = await MConfirm.ShowDialogAsync(this, new(Lang.Get(new(LangKey.Basics_Confirm_Content)))).ConfigureAwait(true);
        ConfirmResult = Lang.Get(new(result ? LangKey.Basics_Confirm_Yes : LangKey.Basics_Confirm_No));
    });
}
