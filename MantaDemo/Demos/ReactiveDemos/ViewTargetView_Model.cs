using Manta.Controls;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ReactiveDemos;

sealed class ViewTargetView_Model : DemoViewModel
{
    protected override MView CreateView()
    {
        return new ViewTargetView();
    }

    public override DemoGroup Group => DemoGroup.Reactive;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override string Header => LangKey.Demo_Reactive_ViewTarget;

    public List<string> Items { get; } = ["One", "Two", "Three", "Four", "Five"];

    public string Current
    {
        get;
        set => SetValue(ref field, value);
    } = string.Empty;
}
