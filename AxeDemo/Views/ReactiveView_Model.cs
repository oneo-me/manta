using Axe.Reactive;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Views;

[View<ReactiveView>]
sealed class ReactiveView_Model : HeaderRegionModel
{
    public static ReactiveView_Model Design => new();

    public override LangKey Header => LangKey.Reactive;
}
