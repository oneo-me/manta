using Axe.Reactive;
using AxeDemo.Controls;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Demos;

[View<ControlsView>]
sealed class ControlsView_Model : HeaderRegionModel
{
    public static ControlsView_Model Design => new();
    public override IconKey Icon => IconKey.Controls;
    public override LangKey Header => LangKey.Controls;
}
