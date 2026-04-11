using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Manta.Controls;
using Manta.Controls.Icons;
using Manta.Globalization;
using Manta.Globalization.Core;
using Manta.Reactive.Commands;
using Manta.Reactive.Extensions;
using Manta.Reactive.Regions;
using Manta.Reactive.Services;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;
using MantaDemo.Services;

namespace MantaDemo.Views;

[View<OverviewView>]
sealed partial class OverviewView_Model : HeaderRegionModel
{
    readonly ConfigService _configService = Service.GetService<ConfigService>();
    readonly RegionManager<HeaderRegionModel> _regionManager;

    public override IconSource Icon => Icons.Home;
    public override string Header => LangKey.Overview;
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

    public CommandWarper<HeaderRegionModel> OpenRegionCommand => GetCommand<HeaderRegionModel>(regionModel =>
    {
        _regionManager.SelectedItem = regionModel;
    });

    public CommandWarper<LangType> ChangeLanguageCommand => GetCommand<LangType>(langType =>
    {
        Lang.Current = langType;
        _configService.Config.Language = langType;
    });
}
