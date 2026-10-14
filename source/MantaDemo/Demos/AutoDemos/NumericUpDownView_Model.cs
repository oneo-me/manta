using Manta.Controls;
using Manta.Reactive.Views;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.AutoDemos;

[View<NumericUpDownView>]
sealed partial class NumericUpDownView_Model : DemoViewModel
{
    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MNumericUpDown);
}
