using Manta.Controls.Icons;
using Manta.Demo.Langs;
using Manta.Demo.Reactive;
using Manta.Reactive.Views;

namespace Manta.Demo.Demos.StyleDemos;

[View<RadiusView>]
sealed class RadiusView_Model : DemoViewModel
{
    public static RingView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource? Icon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Structure--Streamline-Solar-Ar.svg"));
    public override string Header => LangKey.Demo_Style_Radius;
}
