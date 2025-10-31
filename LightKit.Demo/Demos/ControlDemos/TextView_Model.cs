using LightKit.Controls;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Demos.ControlDemos;

[View<TextView>]
sealed class TextView_Model : DemoViewModel
{
    public static TextView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override string Header => nameof(LightText);
}
