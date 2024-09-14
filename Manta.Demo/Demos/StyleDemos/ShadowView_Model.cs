using Manta.Controls.Icons;
using Manta.Demo.Langs;
using Manta.Reactive.Views;
using Manta.Demo.Reactive;

namespace Manta.Demo.Demos.StyleDemos;

[View<ShadowView>]
sealed class ShadowView_Model : DemoViewModel
{
    public static ShadowView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource? Icon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Sticker-Circle--Streamline-Solar-Ar.svg"));
    public override string Header => LangKey.Demo_Style_Shadow;
}
