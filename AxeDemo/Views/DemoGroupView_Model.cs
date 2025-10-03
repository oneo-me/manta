using Axe.Icons;
using Axe.Reactive;
using Axe.Reactive.Views;
using AxeDemo.Interfaces;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Views;

[View<DemoGroupView>]
sealed class DemoGroupView_Model(DemoGroup group, LangKey header, IconSource iconSource) : HeaderRegionModel
{
    public static DemoGroupView_Model Design => new(DemoGroup.Reactive, LangKey.Reactive, Assets.GetIconSource("Icons/Fire--Streamline-Solar-Ar.svg"));

    public override IconSource Icon => iconSource;
    public override LangKey Header => header;
}
