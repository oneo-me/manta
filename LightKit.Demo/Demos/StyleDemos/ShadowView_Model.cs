using LightKit.Controls.Icons;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Demos.StyleDemos;

[View<ShadowView>]
sealed class ShadowView_Model : DemoViewModel
{
    public static ShadowView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override IconSource? Icon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Sticker-Circle--Streamline-Solar-Ar.svg"));
    public override string Header => LangKey.Demo_Style_Shadow;
}
