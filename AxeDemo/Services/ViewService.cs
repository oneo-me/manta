using Axe.Reactive;
using Axe.Reactive.Regions;
using AxeDemo.Interfaces;
using AxeDemo.Langs;
using AxeDemo.Reactive;
using AxeDemo.Views;

namespace AxeDemo.Services;

sealed class ViewService
{
    readonly ConfigService _configService;

    public RegionManager<HeaderRegionModel> RegionManager { get; } = new();

    public ViewService(ConfigService configService)
    {
        _configService = configService;

        RegionManager.Open(() => new OverviewView_Model(RegionManager), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.Styles, LangKey.Styles, Assets.GetIconSource("Icons/Cat--Streamline-Solar-Ar.svg")), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.MultiLanguage, LangKey.MultiLanguage, Assets.GetIconSource("Icons/Planet--Streamline-Solar-Ar.svg")), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.Reactive, LangKey.Reactive, Assets.GetIconSource("Icons/Fire--Streamline-Solar-Ar.svg")), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.Controls, LangKey.Controls, Assets.GetIconSource("Icons/Code-Circle--Streamline-Solar-Ar.svg")), IsCurrent);
    }

    bool IsCurrent(HeaderRegionModel headerRegionModel)
    {
        return string.Equals(_configService.Config.Current, headerRegionModel.Header.ToString());
    }

    public void Save()
    {
        _configService.Config.Current = RegionManager.SelectedItem?.Header.ToString() ?? string.Empty;
    }
}
