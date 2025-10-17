using LightKit.Controls.Icons;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;
using LightKit.Demo.Views;
using LightKit.Reactive.Regions;

namespace LightKit.Demo.Services;

sealed class ViewService
{
    readonly ConfigService _configService;

    public RegionManager<HeaderRegionModel> RegionManager { get; } = new();

    public ViewService(ConfigService configService)
    {
        _configService = configService;

        RegionManager.Open(() => new OverviewView_Model(RegionManager), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.Styles, LangKey.Styles, IconSource.FromAsset(Assets.GetUri("Icons/Cat--Streamline-Solar-Ar.svg"))), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.Reactive, LangKey.Reactive, IconSource.FromAsset(Assets.GetUri("Icons/Fire--Streamline-Solar-Ar.svg"))), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.Controls, LangKey.Controls, IconSource.FromAsset(Assets.GetUri("Icons/Code-Circle--Streamline-Solar-Ar.svg"))), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.MultiLanguage, LangKey.MultiLanguage, IconSource.FromAsset(Assets.GetUri("Icons/Planet--Streamline-Solar-Ar.svg"))), IsCurrent);
    }

    bool IsCurrent(HeaderRegionModel headerRegionModel)
    {
        return string.Equals(_configService.Config.Current, headerRegionModel.Header.ToString());
    }

    public void Save()
    {
        _configService.Config.Current = RegionManager.SelectedItem?.Header.ToString() ?? string.Empty;
        RegionManager.Dispose();
    }
}
