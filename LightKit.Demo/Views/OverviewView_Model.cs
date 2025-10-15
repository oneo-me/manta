using System.Collections.ObjectModel;
using System.Reactive.Linq;
using LightKit.Controls.Icons;
using LightKit.Globalization;
using LightKit.Globalization.Core;
using LightKit.Reactive.Extensions;
using LightKit.Reactive.Regions;
using LightKit.Reactive.Services;
using LightKit.Reactive.Views;
using LightKit.Demo.Langs;
using DynamicData;
using DynamicData.Binding;
using LightKit.Demo.Reactive;
using LightKit.Demo.Services;

namespace LightKit.Demo.Views;

[View<OverviewView>]
sealed class OverviewView_Model : HeaderRegionModel
{
    public static OverviewView_Model Design => new(new(new DemoGroupView_Model(DemoGroup.Styles, LangKey.Styles, Assets.GetIconSource("Icons/Cat--Streamline-Solar-Ar.svg"))));

    readonly ConfigService _configService = Service.GetService<ConfigService>();
    readonly RegionManager<HeaderRegionModel> _regionManager;

    public override IconSource Icon { get; } = Assets.GetIconSource("Icons/Home-Smile-Angle--Streamline-Solar-Ar.svg");
    public override LangKey Header => LangKey.Overview;
    public ReadOnlyObservableCollection<HeaderRegionModel> Items { get; }

    public LangType[] SupportedLanguages { get; } = [LangType.en, LangType.zh_Hans];

    public OverviewView_Model(RegionManager<HeaderRegionModel> regionManager)
    {
        _regionManager = regionManager;
        _regionManager.Items
            .ToObservableChangeSet()
            .Synchronize()
            .ObserveOnDispatcher()
            .Filter(x => x.Header != LangKey.Overview)
            .Bind(out var items)
            .Subscribe();

        Items = items;
    }

    public void OpenRegion(HeaderRegionModel regionModel)
    {
        _regionManager.SelectedItem = regionModel;
    }

    public void ChangeLanguage(LangType langType)
    {
        Lang.Current = langType;
        _configService.Config.Language = langType;
    }
}
