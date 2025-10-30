using LightKit.Controls;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Demos.ControlDemos;

[View<LabelView>]
sealed class LabelView_Model : DemoViewModel
{
    public static LabelView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override string Header => LangKey.Demo_Control_Title;
    public override object HeaderArg => nameof(LightLabel);
}
