using LightKit.Controls;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Demos.ControlDemos;

[View<ProgressRingView>]
sealed class ProgressRingView_Model : DemoViewModel
{
    public static ProgressRingView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override string Header => nameof(LightProgressRing);
}
