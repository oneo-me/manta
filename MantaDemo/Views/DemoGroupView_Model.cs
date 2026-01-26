using Manta.Controls.Icons;
using Manta.Globalization;
using Manta.Reactive.Services;
using Manta.Reactive.Views;
using MantaDemo.Langs;
using MantaDemo.Reactive;
using MantaDemo.Services;

namespace MantaDemo.Views;

interface IDemoItem
{
    int Count { get; set; }
    bool IsGroup { get; }
}

sealed class DemoItem(DemoViewModel viewModel) : IDemoItem
{
    public DemoViewModel ViewModel { get; } = viewModel;
    public int Count { get; set; }
    public bool IsGroup => false;
}

sealed class DemoGroupItem(DemoSubGroup subGroup) : Model, IDemoItem
{
    public DemoSubGroup SubGroup { get; } = subGroup;
    public string GroupName { get; } = Lang.CreateKey(typeof(App).Assembly, $"SubGroup_{subGroup}");
    public bool IsGroup => true;

    public int Count
    {
        get;
        set => SetValue(ref field, value);
    }
}

[View<DemoGroupView>]
sealed class DemoGroupView_Model : HeaderRegionModel
{
    public static DemoGroupView_Model Design => new(DemoGroup.Controls, LangKey.Controls, Icons.Controls);

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
        DemoGroupItem? currentGroup = null;
        foreach (var demo in demos)
        {
            if (list.OfType<DemoGroupItem>().All(x => x.SubGroup != demo.SubGroup) && demo.SubGroup != DemoSubGroup.None)
            {
                currentGroup = new(demo.SubGroup);
                list.Add(currentGroup);
            }

            list.Add(new DemoItem(demo));
            currentGroup?.Count += 1;
        }

        Demos = list;

        if (_configService.Config.LastDemos.TryGetValue(group, out var item))
            CurrentDemo = Demos.OfType<DemoItem>().FirstOrDefault(x => string.Equals(item, x.ViewModel.Header.ToString()));

        CurrentDemo ??= Demos.OfType<DemoItem>().FirstOrDefault();
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
