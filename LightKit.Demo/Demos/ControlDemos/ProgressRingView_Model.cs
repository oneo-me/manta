using LightKit.Controls;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Demos.ControlDemos;

[View<ProgressRingView>]
sealed class ProgressRingView_Model : DemoViewModel
{
    public static ProgressRingView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override LangKey Header => LangKey.Demo_Control_Title;
    public override object HeaderArg => nameof(LightProgressRing);
}
