using System.Diagnostics;
using System.Reactive.Disposables;
using AirKit.Attributes;
using AirKit.Controls;
using AirKit.Demo.Models;
using AirKit.Demo.Services;
using AirKit.Extensions;
using Avalonia.Controls;
using Avalonia.Media;

namespace AirKit.Demo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
    readonly ConfigService _configService = Service.GetService<ConfigService>();

    public RegionManager<SampleRegionModel> RegionManager { get; } = new();

    public Main_Model()
    {
        var baseType = typeof(SampleRegionModel);
        foreach (var type in GetType().Assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface || type.IsAssignableTo(baseType) is false)
                continue;

            RegionManager.Open(() => Service.CreateInstance<SampleRegionModel>(type), _ => false);
        }

        RegionManager.SelectedItem = RegionManager.Items.FirstOrDefault(x => string.Equals(x.Header, _configService.Config.SelectedSample, StringComparison.Ordinal)) ?? RegionManager.Items.FirstOrDefault();
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var view = this.GetView<Main>();

        if (Design.IsDesignMode)
        {
            view.MoveBorder1.Background = new SolidColorBrush(Color.FromArgb((byte)(2.55 * 10), 0, 0, 0));
            view.MoveBorder2.Background = new SolidColorBrush(Color.FromArgb((byte)(2.55 * 10), 0, 0, 0));
        }

        view.SubscribeTitleBarEvents(x => x.MoveBorder1).DisposeWith(disposables);
        view.SubscribeTitleBarEvents(x => x.MoveBorder2).DisposeWith(disposables);

        view.SubscribeWindowStatus(x =>
        {
            view.MoveBorder1.Height = x.topOffset > 0 ? x.topOffset - 6 : 0;
            view.MoveBorder2.IsVisible = (Debugger.IsAttached && x.isFullscreen is false) || x.isShowButtons;
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
            _configService.Config.SelectedSample = RegionManager.SelectedItem?.Header;
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
