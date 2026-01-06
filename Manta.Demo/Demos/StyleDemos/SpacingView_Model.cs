using Manta.Controls.Icons;
using Manta.Demo.Langs;
using Manta.Demo.Reactive;
using Manta.Reactive.Views;

namespace Manta.Demo.Demos.StyleDemos;

[View<SpacingView>]
sealed class SpacingView_Model: DemoViewModel
{
    public static ThicknessView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource? Icon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Posts-Carousel-Horizontal--Streamline-Solar-Ar.svg"));
    public override string Header => LangKey.Demo_Style_Spacing;
}
