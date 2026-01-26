using Manta.Controls;
using Manta.Reactive.Views;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ControlDemos;

[View<IconView>]
sealed class IconView_Model : DemoViewModel
{
    public static IconView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Custom;
    public override string Header => nameof(MIcon);
}
