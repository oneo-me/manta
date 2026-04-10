using Manta.Controls;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.AutoDemos;

sealed class ScrollViewerView_Model : DemoViewModel
{
    protected override MView CreateView()
    {
        return new ScrollViewerView();
    }

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MScrollViewer);
}
