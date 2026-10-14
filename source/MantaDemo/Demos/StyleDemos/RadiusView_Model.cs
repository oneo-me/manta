using Manta.Controls.Icons;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.StyleDemos;

[View<RadiusView>]
sealed partial class RadiusView_Model : DemoViewModel
{
    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource Icon => Icons.Radius;
    public override string Header => LangKey.Demo_Style_Radius;
}
