using Axe.Reactive;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Views;

[View<ControlsView>]
sealed class ControlsView_Model : HeaderRegionModel
{
    public static ControlsView_Model Design => new();
    public override LangKey Header => LangKey.Controls;
}
