using Manta.Controls;
using Manta.Controls.Icons;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.StyleDemos;

sealed class ShadowView_Model : DemoViewModel
{
    protected override MView CreateView()
    {
        return new ShadowView();
    }

    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource Icon => Icons.Shadow;
    public override string Header => LangKey.Demo_Style_Shadow;
}
