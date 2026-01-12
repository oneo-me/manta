using Manta.Controls.Icons;
using Manta.Demo.Langs;
using Manta.Demo.Reactive;
using Manta.Demo.Services;
using Manta.Globalization;
using Manta.Reactive.Services;
using Manta.Reactive.Views;

namespace Manta.Demo.Views;

interface IDemoItem
{
    bool IsGroup { get; }
}

sealed class DemoItem(DemoViewModel viewModel) : IDemoItem
{
    public DemoViewModel ViewModel { get; } = viewModel;
    public bool IsGroup => false;
}

sealed class DemoGroupItem(DemoSubGroup subGroup) : IDemoItem
{
    public DemoSubGroup SubGroup { get; } = subGroup;
    public string GroupName { get; } = Lang.CreateKey(typeof(App).Assembly, $"SubGroup_{subGroup}");
    public bool IsGroup => true;
}

[View<DemoGroupView>]
sealed class DemoGroupView_Model : HeaderRegionModel
{
    public static DemoGroupView_Model Design => new(DemoGroup.Controls, LangKey.Controls, IconSource.FromAsset(Assets.GetUri("Icons/Fire--Streamline-Solar-Ar.svg")));

    readonly ConfigService _configService = Service.GetService<ConfigService>();
    readonly DemoService _demoService = Service.GetService<DemoService>();

    readonly DemoGroup _group;

    public override IconSource Icon { get; }
    public override string Header { get; }
    public IReadOnlyCollection<IDemoItem> Demos { get; }

    public IDemoItem? CurrentDemo
    {
        get;
        set
        {
            if (SetValue(ref field, value))
            {
                if (value is DemoItem demoItem)
                    CurrentViewModel = demoItem.ViewModel;
                else
                    CurrentViewModel = null;
            }
        }
    }

    public ViewModel? CurrentViewModel
    {
        get;
        private set => SetValue(ref field, value);
    }

    public DemoGroupView_Model(DemoGroup group, string header, IconSource iconSource)
    {
        _group = group;
        Header = header;
        Icon = iconSource;

        var list = new List<IDemoItem>();

        var demos = _demoService.GetDemos(group);
        foreach (var demo in demos)
        {
            if (list.OfType<DemoGroupItem>().All(x => x.SubGroup != demo.SubGroup) && demo.SubGroup != DemoSubGroup.None)
                list.Add(new DemoGroupItem(demo.SubGroup));
            list.Add(new DemoItem(demo));
        }

        Demos = list;

        if (_configService.Config.LastDemos.TryGetValue(group, out var item))
            CurrentDemo = Demos.OfType<DemoItem>().FirstOrDefault(x => string.Equals(item, x.ViewModel.Header.ToString()));

        CurrentDemo ??= Demos.OfType<DemoItem>().FirstOrDefault();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        this.SetContext("header", Header);
    }

    protected override void OnClosed()
    {
        base.OnClosed();

        if (CurrentDemo is null)
            _configService.Config.LastDemos.Remove(_group);
        else
            _configService.Config.LastDemos[_group] = CurrentDemo is DemoItem demoItem ? demoItem.ViewModel.Header : string.Empty;
    }
}
