using Axe.Icons;
using Axe.Reactive;
using Axe.Reactive.Views;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Demos;

[View<ColorView>]
sealed class ColorView_Model : DemoViewModel
{
    public static ColorView_Model Design => new();

    public override IconSource Icon { get; } = Assets.GetIconSource("Icons/Fire--Streamline-Solar-Ar.svg");
    public override LangKey Header => LangKey.Reactive;
    public override DemoGroup Group => DemoGroup.Styles;
}
