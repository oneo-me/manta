using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Axe.I18N;
using Axe.I18N.Core;
using Axe.Reactive.Extensions;
using Axe.Reactive.Services;
using Axe.Reactive.Views;
using ControlsDemo.Langs;
using ControlsDemo.Models;
using ControlsDemo.Services;
using DynamicData;

namespace ControlsDemo.Views;

[View<Main>]
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable")]
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
                ViewManager.OpenView<ViewModel>(value.Type);
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

        var samples = new List<(SampleGroupType groupType, LangKey nameKey, Type type)>();
        var baseType = typeof(ViewModel);

        foreach (var type in GetType().Assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface || type.IsAssignableTo(baseType) is false)
                continue;

            if (Attribute.GetCustomAttribute(type, typeof(SampleAttribute)) is not SampleAttribute sampleAttribute)
                continue;

            samples.Add((sampleAttribute.GroupType, sampleAttribute.NameKey, type));
        }

        var groupings = samples.GroupBy(x => x.groupType).ToList();
        groupings.Sort((x, y) => x.Key.CompareTo(y.Key));

        foreach (var grouping in groupings)
        {
            _sourceList.Add(new(GetGroupName(grouping.Key)));

            foreach (var (_, name, type) in grouping)
                _sourceList.Add(new(name, type));
        }

        SelectedItem = _sourceList.Items.FirstOrDefault(x => x.IsGroup is false && x.NameKey == _configService.Config.SelectedSample) ?? _sourceList.Items.FirstOrDefault(x => x.IsGroup is false);
    }

    static readonly Dictionary<SampleGroupType, LangKey> _groupNameCache = new();
    static readonly Type _sampleGroupType = typeof(SampleGroupType);

    static LangKey GetGroupName(SampleGroupType groupType)
    {
        if (_groupNameCache.TryGetValue(groupType, out var groupNameKey))
            return groupNameKey;

        var fieldInfo = _sampleGroupType.GetField(groupType.ToString());
        if (fieldInfo is null)
            throw new InvalidOperationException();

        if (Attribute.GetCustomAttribute(fieldInfo, typeof(SampleGroupAttribute)) is not SampleGroupAttribute sampleGroupAttribute)
            throw new InvalidOperationException();

        return _groupNameCache[groupType] = sampleGroupAttribute.NameKey;
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void SetLangType(LangType langType)
    {
        Lang.Current = langType;
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var view = this.GetView<Main>();

        view.SubscribeTitleBarEvents(x => x.EventBorder).DisposeWith(disposables);

        view.SubscribeWindowStatus(x =>
        {
            view.OffsetBorder.Height = x.topOffset > 0 ? x.topOffset - 12 : 0;
        }).DisposeWith(disposables);
    }

    public override void OnWindowInitialized(Window window)
    {
        base.OnWindowInitialized(window);

        window.WindowState = _configService.Config.MainWindow.WindowState;
        window.Position = new(_configService.Config.MainWindow.Left, _configService.Config.MainWindow.Top);
        window.Width = _configService.Config.MainWindow.Width;
        window.Height = _configService.Config.MainWindow.Height;

        window.Closing += delegate
        {
            _configService.Config.Lang = Lang.Current;

            _configService.Config.SelectedSample = SelectedItem?.NameKey;

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
