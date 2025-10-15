using LightKit.Controls.Icons;
using LightKit.Reactive.Services;
using LightKit.Reactive.Views;
using LightKit.Demo.Langs;
using LightKit.Demo.Reactive;
using LightKit.Demo.Services;

namespace LightKit.Demo.Views;

[View<DemoGroupView>]
sealed class DemoGroupView_Model : HeaderRegionModel
{
    public static DemoGroupView_Model Design => new(DemoGroup.Styles, LangKey.Styles, Assets.GetIconSource("Icons/Fire--Streamline-Solar-Ar.svg"));

    readonly ConfigService _configService = Service.GetService<ConfigService>();
    readonly DemoService _demoService = Service.GetService<DemoService>();

    readonly DemoGroup _group;

    public override IconSource Icon { get; }
    public override LangKey Header { get; }
    public IReadOnlyCollection<DemoViewModel> Demos { get; }

    public DemoViewModel? CurrentDemo
    {
        get;
        set => SetValue(ref field, value);
    }

    public DemoGroupView_Model(DemoGroup group, LangKey header, IconSource iconSource)
    {
        _group = group;
        Header = header;
        Icon = iconSource;
        Demos = _demoService.GetDemos(group);

        if (_configService.Config.LastDemos.TryGetValue(group, out var item))
            CurrentDemo = Demos.FirstOrDefault(x => string.Equals(item, x.Header.ToString()));

        CurrentDemo ??= Demos.FirstOrDefault();
    }

    protected override void OnClosed()
    {
        base.OnClosed();

        if (CurrentDemo is null)
            _configService.Config.LastDemos.Remove(_group);
        else
            _configService.Config.LastDemos[_group] = CurrentDemo.Header.ToString();
    }
}
