using LightKit.Controls.Icons;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;
using LightKit.Reactive.Views;

namespace LightKit.Demo.Demos.StyleDemos;

[View<FontView>]
sealed class FontView_Model : DemoViewModel
{
    public static FontView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override IconSource? Icon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Text-Circle--Streamline-Solar-Ar.svg"));
    public override LangKey Header => LangKey.Demo_Style_Font;
}
