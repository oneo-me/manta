using Manta.Reactive.Views;

namespace MantaDemo.Demos.ReactiveDemos;

[View<ViewTargetViewChild>]
sealed class ViewTargetViewChild_Model : ViewModel, IViewHostTarget<string>
{
    public static ViewTargetViewChild_Model Design => new();

    public string Data
    {
        get;
        set => SetValue(ref field, value);
    } = string.Empty;

    public TimeSpan Time
    {
        get;
        set => SetValue(ref field, value);
    } = TimeSpan.Zero;

    public void SetData(string data)
    {
        Data = data;
        Time = DateTimeOffset.Now.TimeOfDay;
    }
}
