using Manta.Controls;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.AutoDemos;

sealed class MRepeatButtonView_Model : DemoViewModel
{
    protected override MView CreateView()
    {
        return new MRepeatButtonView();
    }

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MRepeatButton);
}
