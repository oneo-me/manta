using Manta.Controls;
using Manta.Controls.Icons;
using Manta.Reactive.Views;
using Manta.Demo.Reactive;

namespace Manta.Demo.Demos.ControlDemos;

[View<MtIconView>]
sealed class LightIconView_Model : DemoViewModel
{
    public static LightIconView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Icon;
    public override string Header => nameof(MtIcon);

    public IconSource TestIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Home-Smile-Angle--Streamline-Solar-Ar.svg"));
}
