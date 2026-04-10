using Manta.Controls;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.AutoDemos;

sealed class AspectRatioView_Model : DemoViewModel
{
    protected override MView CreateView()
    {
        return new AspectRatioView();
    }

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MAspectRatio);
}
