using Axe.Icons;
using Axe.Reactive;
using Axe.Reactive.Views;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Demos;

[View<ControlsView>]
sealed class ControlsView_Model : HeaderRegionModel
{
    public static ControlsView_Model Design => new();
    public override IconSource Icon { get; } = Assets.GetIconSource("Icons/Code-Circle--Streamline-Solar-Ar.svg");
    public override LangKey Header => LangKey.Controls;
}
