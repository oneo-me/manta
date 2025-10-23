using LightKit.Controls;
using LightKit.Controls.Icons;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Demos.ControlDemos;

[View<ButtonView>]
sealed class ButtonView_Model : DemoViewModel
{
    public static ButtonView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override LangKey Header => LangKey.Demo_Control_Title;
    public override object HeaderArg => nameof(LightButton);

    public IconSource TestIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Palette-Round--Streamline-Solar-Ar.svg"));
}
