using Manta.Controls;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.AutoDemos;

sealed class TitleBarView_Model : DemoViewModel
{
    protected override MView CreateView()
    {
        return new TitleBarView();
    }

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Window;
    public override string Header => nameof(MTitleBar);
}
