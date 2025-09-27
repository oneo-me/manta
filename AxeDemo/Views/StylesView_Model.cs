using Axe.Reactive;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Views;

[View<StylesView>]
sealed class StylesView_Model : HeaderRegionModel
{
    public static StylesView_Model Design => new();
    public override LangKey Header => LangKey.Styles;
}
