using Manta;
using Manta.Controls.Icons;
using MantaDemo.Langs;
using Manta.Reactive.Views;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.StyleDemos;

[View<ThicknessView>]
sealed class ThicknessView_Model : DemoViewModel
{
    public static ThicknessView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource? Icon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Minimize-Square-2--Streamline-Solar-Ar.svg"));
    public override string Header => LangKey.Demo_Style_Thickness;
}
