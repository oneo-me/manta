using LightKit.Controls.Icons;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Demos.StyleDemos;

[View<ColorView>]
sealed class ColorView_Model : DemoViewModel
{
    public static ColorView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override IconSource? Icon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Palette-Round--Streamline-Solar-Ar.svg"));
    public override LangKey Header => LangKey.Demo_Style_Color;
}
