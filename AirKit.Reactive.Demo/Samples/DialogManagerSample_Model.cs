using AirKit.Reactive.Attributes;
using AirKit.Reactive.Demo.Models;

namespace AirKit.Reactive.Demo.Samples;

[View<DialogManagerSample>]
sealed class DialogManagerSample_Model : SampleTabModel
{
    public DialogManager<DialogModel> DialogManager { get; } = new();

    public DialogManagerSample_Model()
    {
        Header = "DialogManager";
    }

    protected override void OnClosed()
    {
        base.OnClosed();
        DialogManager.Dispose();
    }
}
