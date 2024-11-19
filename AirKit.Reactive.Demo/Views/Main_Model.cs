using AirKit.Reactive.Attributes;
using AirKit.Reactive.Demo.Models;
using AirKit.Reactive.Demo.Services;
using Avalonia.Controls;

namespace AirKit.Reactive.Demo.Views;

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

    public override void OnWindowInitialized(Window window)
    {
        base.OnWindowInitialized(window);

        window.Title = LightApp.Current.Name;

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
