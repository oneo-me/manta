using Manta.Controls;
using Manta.Controls.Icons;
using Manta.Demo.Reactive;
using Manta.Reactive.Views;

namespace Manta.Demo.Demos.ControlDemos;

[View<IconView>]
sealed class IconView_Model : DemoViewModel
{
    public static IconView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MIcon);

    public IconSource TestIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Home-Smile-Angle--Streamline-Solar-Ar.svg"));
}
