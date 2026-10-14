using Manta.Controls;
using Manta.Reactive.Views;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.AutoDemos;

[View<TitleBarView>]
sealed partial class TitleBarView_Model : DemoViewModel
{
    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Window;
    public override string Header => nameof(MTitleBar);
}
