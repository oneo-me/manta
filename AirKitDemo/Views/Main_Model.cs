using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Disposables;
using AirKit;
using AirKit.Attributes;
using AirKit.Controls;
using AirKit.Extensions;
using AirKitDemo.Models;
using AirKitDemo.Services;
using Avalonia.Controls;
using DynamicData;

namespace AirKitDemo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
    readonly ConfigService _configService = Service.GetService<ConfigService>();
    readonly SourceList<SampleItem> _sourceList = new();

    public ReadOnlyObservableCollection<SampleItem> Items { get; }

    public SampleItem? SelectedItem
    {
        get;
        set
        {
            if (SetValue(ref field, value) is false)
                return;

            if (value?.Type is null)
                ViewManager.ClearView();
            else
                ViewManager.OpenView(value.Type);
        }
    }

    public ViewManager ViewManager { get; } = new();

    public Main_Model()
    {
        _sourceList.Connect()
            .ObserveOnDispatcher()
            .Bind(out var items)
            .Subscribe();

        Items = items;

        var samples = new List<(SampleGroupType groupType, string name, Type type)>();
        var baseType = typeof(SampleRegionModel);

        foreach (var type in GetType().Assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface || type.IsAssignableTo(baseType) is false)
                continue;

            if (Attribute.GetCustomAttribute(type, typeof(SampleAttribute)) is not SampleAttribute sampleAttribute)
                continue;

            samples.Add((sampleAttribute.GroupType, sampleAttribute.Name, type));
        }

        var groupings = samples.GroupBy(x => x.groupType).ToList();
        groupings.Sort((x, y) => x.Key.CompareTo(y.Key));

        foreach (var grouping in groupings)
        {
            _sourceList.Add(new(GetGroupName(grouping.Key)));

            foreach (var (_, name, type) in grouping)
                _sourceList.Add(new(name, type));
        }

        SelectedItem = _sourceList.Items.FirstOrDefault(x => x.IsGroup is false && x.Name == _configService.Config.SelectedSample) ?? _sourceList.Items.FirstOrDefault(x => x.IsGroup is false);
    }

    static readonly Dictionary<SampleGroupType, string> _groupNameCache = new();
    static readonly Type _sampleGroupType = typeof(SampleGroupType);

    static string GetGroupName(SampleGroupType groupType)
    {
        if (_groupNameCache.TryGetValue(groupType, out var groupName))
            return groupName;

        var fieldInfo = _sampleGroupType.GetField(groupType.ToString());
        if (fieldInfo is null)
            throw new InvalidOperationException();

        if (Attribute.GetCustomAttribute(fieldInfo, typeof(SampleGroupAttribute)) is not SampleGroupAttribute sampleGroupAttribute)
            throw new InvalidOperationException();

        return _groupNameCache[groupType] = sampleGroupAttribute.Name;
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var view = this.GetView<Main>();

        view.SubscribeTitleBarEvents(x => x.MoveBorder1).DisposeWith(disposables);
        view.SubscribeTitleBarEvents(x => x.MoveBorder2).DisposeWith(disposables);

        view.SubscribeWindowStatus(x =>
        {
            view.MoveBorder1.Height = x.topOffset > 0 ? x.topOffset - 6 : 0;
            view.Buttons.IsVisible = (Debugger.IsAttached && x.isFullscreen is false) || x.isShowButtons;
            view.MoveBorder2.Margin = new(0, 0, 0, -12);
            view.MoveBorder2.Height = x.titleBarHeight;
        }).DisposeWith(disposables);
    }

    public override void OnWindowInitialized(LightWindow window)
    {
        base.OnWindowInitialized(window);

        window.WindowState = _configService.Config.MainWindow.WindowState;
        window.Position = new(_configService.Config.MainWindow.Left, _configService.Config.MainWindow.Top);
        window.Width = _configService.Config.MainWindow.Width;
        window.Height = _configService.Config.MainWindow.Height;

        window.Closing += delegate
        {
            _configService.Config.SelectedSample = SelectedItem?.Name;
            _configService.Config.MainWindow.WindowState = window.WindowState;
            _configService.Config.MainWindow.Left = window.Position.X;
            _configService.Config.MainWindow.Top = window.Position.Y;

            if (window.WindowState == WindowState.Normal)
            {
                _configService.Config.MainWindow.Width = window.Width;
                _configService.Config.MainWindow.Height = window.Height;
            }
        };
    }
}
