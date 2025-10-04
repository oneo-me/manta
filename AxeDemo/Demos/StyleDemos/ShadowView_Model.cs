using Axe.Icons;
using Axe.Reactive;
using Axe.Reactive.Views;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Demos.StyleDemos;

[View<ShadowView>]
sealed class ShadowView_Model : DemoViewModel
{
    public static ShadowView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override IconSource? Icon { get; } = Assets.GetIconSource("Icons/Sticker-Circle--Streamline-Solar-Ar.svg");
    public override LangKey Header => LangKey.Demo_Style_Shadow;
}
