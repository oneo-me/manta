using LightKit.Controls;
using LightKit.Controls.Icons;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Demos.ControlDemos;

[View<LightIconView>]
sealed class LightIconView_Model : DemoViewModel
{
    public static LightIconView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Icon;
    public override string Header => nameof(LightIcon);

    public IconSource TestIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Home-Smile-Angle--Streamline-Solar-Ar.svg"));
}
