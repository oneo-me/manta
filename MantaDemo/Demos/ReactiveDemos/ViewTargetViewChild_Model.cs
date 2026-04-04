using Manta.Controls;
using Manta.Reactive.Views;

namespace MantaDemo.Demos.ReactiveDemos;

sealed class ViewTargetViewChild_Model : ViewModel, IViewHostTarget<string>
{
    protected override MView CreateView()
    {
        return new ViewTargetViewChild();
    }

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
