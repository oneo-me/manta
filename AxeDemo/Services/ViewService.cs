using Axe.Reactive.Regions;
using AxeDemo.Demos;
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
        RegionManager.Open(() => new StylesView_Model(), IsCurrent);
        RegionManager.Open(() => new ReactiveView_Model(), IsCurrent);
        RegionManager.Open(() => new MultiLanguageView_Model(), IsCurrent);
        RegionManager.Open(() => new ControlsView_Model(), IsCurrent);
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
