using Axe.Reactive;
using AxeDemo.Controls;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Demos;

[View<StylesView>]
sealed class StylesView_Model : HeaderRegionModel
{
    public static StylesView_Model Design => new();

    public override IconKey Icon => IconKey.Styles;
    public override LangKey Header => LangKey.Styles;
}
