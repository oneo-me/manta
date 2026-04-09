using Avalonia.Controls;
using Manta.Controls;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.AutoDemos;

sealed class ToolTipView_Model : DemoViewModel
{
    protected override MView CreateView()
    {
        return new ToolTipView();
    }

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Base;
    public override string Header => nameof(ToolTip);
}
