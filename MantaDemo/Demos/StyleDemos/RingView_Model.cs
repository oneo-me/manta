using Manta.Controls.Icons;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.StyleDemos;

[View<RingView>]
sealed class RingView_Model : DemoViewModel
{
    public static RingView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource Icon => Icons.Ring;
    public override string Header => LangKey.Demo_Style_Ring;
}
