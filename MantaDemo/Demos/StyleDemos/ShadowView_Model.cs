using Manta.Controls.Icons;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.StyleDemos;

[View<ShadowView>]
sealed class ShadowView_Model : DemoViewModel
{
    public static ShadowView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource Icon => Icons.Shadow;
    public override string Header => LangKey.Demo_Style_Shadow;
}
