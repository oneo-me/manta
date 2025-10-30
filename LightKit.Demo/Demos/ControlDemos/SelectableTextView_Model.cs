using LightKit.Controls;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Demos.ControlDemos;

[View<SelectableTextView>]
sealed class SelectableTextView_Model : DemoViewModel
{
    public static SelectableTextView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override string Header => LangKey.Demo_Control_Title;
    public override object HeaderArg => nameof(LightSelectableText);
}
