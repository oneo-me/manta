using Axe.Icons;
using Axe.Reactive.Views;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Demos.StyleDemos;

[View<FontView>]
sealed class FontView_Model : DemoViewModel
{
    public static FontView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Styles;
    public override IconSource? Icon => null;
    public override LangKey Header => LangKey.Demo_Style_Font;
}
