using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ReactiveDemos;

[View<ViewTargetView>]
sealed partial class ViewTargetView_Model : DemoViewModel
{
    public override DemoGroup Group => DemoGroup.Reactive;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override string Header => LangKey.Demo_Reactive_ViewTarget;

    public List<string> Items { get; } = ["One", "Two", "Three", "Four", "Five"];

    public string Current
    {
        get;
        set => SetField(ref field, value);
    } = string.Empty;
}
