using Manta.Controls.Icons;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.StyleDemos;

[View<ColorView>]
sealed partial class ColorView_Model : DemoViewModel
{
    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource Icon => Icons.Color;
    public override string Header => LangKey.Demo_Style_Color;
}
