using Manta.Controls;
using Manta.Controls.Icons;
using MantaDemo.Langs;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.StyleDemos;

sealed class SpacingView_Model : DemoViewModel
{
    protected override MView CreateView()
    {
        return new SpacingView();
    }

    public override DemoGroup Group => DemoGroup.Styles;
    public override DemoSubGroup SubGroup => DemoSubGroup.None;
    public override IconSource Icon => Icons.Spacing;
    public override string Header => LangKey.Demo_Style_Spacing;
}
