using Axe.Reactive;
using AxeDemo.Langs;
using AxeDemo.Reactive;

namespace AxeDemo.Views;

[View<OverviewView>]
sealed class OverviewView_Model : HeaderRegionModel
{
    public static OverviewView_Model Design => new();

    public override LangKey Header => LangKey.Overview;
}
