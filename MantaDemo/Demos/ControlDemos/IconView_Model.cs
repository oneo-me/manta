using Manta.Controls;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ControlDemos;

sealed class IconView_Model : DemoViewModel
{
    protected override MView CreateView()
    {
        return new IconView();
    }

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Custom;
    public override string Header => nameof(MIcon);
}
