using Axe.Icons;
using Axe.Reactive;
using Axe.Reactive.Views;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Demos;

[View<StylesView>]
sealed class StylesView_Model : HeaderRegionModel
{
    public static StylesView_Model Design => new();

    public override IconSource Icon { get; } = Assets.GetIconSource("Icons/Cat--Streamline-Solar-Ar.svg");
    public override LangKey Header => LangKey.Styles;
}
