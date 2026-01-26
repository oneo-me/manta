using Manta.Controls.Icons;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.StyleDemos;

[View<ThicknessView>]
sealed class ThicknessView_Model : DemoViewModel
{
    public static ThicknessView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource Icon => Icons.Thickness;
    public override string Header => LangKey.Demo_Style_Thickness;
}
