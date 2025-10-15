using LightKit.Controls.Icons;
using LightKit.Reactive.Views;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;

namespace LightKit.Demo.Demos.StyleDemos;

[View<RadiusView>]
sealed class RadiusView_Model : DemoViewModel
{
    public static RadiusView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override IconSource? Icon { get; } = Assets.GetIconSource("Icons/Structure--Streamline-Solar-Ar.svg");
    public override LangKey Header => LangKey.Demo_Style_Radius;
}
