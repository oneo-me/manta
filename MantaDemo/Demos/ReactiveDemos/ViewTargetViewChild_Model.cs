using Manta.Reactive.Views;

namespace MantaDemo.Demos.ReactiveDemos;

[View<ViewTargetViewChild>]
sealed partial class ViewTargetViewChild_Model : ViewModel, IViewHostTarget<string>
{
    public string Data
    {
        get;
        set => SetField(ref field, value);
    } = string.Empty;

    public TimeSpan Time
    {
        get;
        set => SetField(ref field, value);
    } = TimeSpan.Zero;

    public void SetData(string data)
    {
        Data = data;
        Time = DateTimeOffset.Now.TimeOfDay;
    }
}
