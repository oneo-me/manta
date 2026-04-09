using Manta.Reactive.Regions;
using MantaDemo.Langs;
using MantaDemo.Reactive;
using MantaDemo.Views;

namespace MantaDemo.Services;

sealed class ViewService : IDisposable
{
    readonly ConfigService _configService;

    public RegionManager<HeaderRegionModel> RegionManager { get; } = new();

    public ViewService(ConfigService configService)
    {
        _configService = configService;

        RegionManager.Open(() => new OverviewView_Model(RegionManager), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.Styles, LangKey.Styles, Icons.Styles), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.Reactive, LangKey.Reactive, Icons.Reactive), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.Controls, LangKey.Controls, Icons.Controls), IsCurrent);
        RegionManager.Open(() => new DemoGroupView_Model(DemoGroup.MultiLanguage, LangKey.MultiLanguage, Icons.MultiLanguage), IsCurrent);
    }

    bool IsCurrent(HeaderRegionModel headerRegionModel)
    {
        return string.Equals(_configService.Config.Current, headerRegionModel.Header);
    }

    public void Dispose()
    {
        _configService.Config.Current = RegionManager.SelectedItem?.Header ?? string.Empty;
        RegionManager.Dispose();
    }
}
