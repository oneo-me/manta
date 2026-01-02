using Manta.Controls.Icons;
using Manta.Demo.Langs;
using Manta.Demo.Reactive;
using Manta.Reactive.Views;

namespace Manta.Demo.Demos.StyleDemos;

[View<ColorView>]
sealed class ColorView_Model : DemoViewModel
{
    public static ColorView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource? Icon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Palette-Round--Streamline-Solar-Ar.svg"));
    public override string Header => LangKey.Demo_Style_Color;
}
