using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Axe.I18N;
using Axe.I18N.Core;
using Axe.Reactive.Extensions;
using Axe.Reactive.Regions;
using Axe.Reactive.Services;
using Axe.Reactive.Views;
using AxeDemo.Controls;
using AxeDemo.Demos;
using AxeDemo.Langs;
using AxeDemo.Reactive;
using AxeDemo.Services;
using DynamicData;
using DynamicData.Binding;

namespace AxeDemo.Views;

[View<OverviewView>]
sealed class OverviewView_Model : HeaderRegionModel
{
    public static OverviewView_Model Design => new(new(new ControlsView_Model(), new MultiLanguageView_Model(), new StylesView_Model(), new ReactiveView_Model()));

    readonly ConfigService _configService = Service.GetService<ConfigService>();
    readonly RegionManager<HeaderRegionModel> _regionManager;

    public override IconKey Icon => IconKey.Overview;
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
