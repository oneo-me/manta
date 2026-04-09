using Manta.Controls;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.AutoDemos;

sealed class MTabTitleBarView_Model : DemoViewModel
{
    protected override MView CreateView()
    {
        return new MTabTitleBarView();
    }

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Window;
    public override string Header => nameof(MTabTitleBar);
}
