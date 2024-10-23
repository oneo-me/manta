using AirKit.Reactive.Attributes;
using AirKit.Reactive.Demo.Models;
using AirKit.Reactive.Demo.Services;
using AirKit.Reactive.Extensions;
using Avalonia.Controls;
using R3;

namespace AirKit.Reactive.Demo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
    readonly ConfigService _configService = Service.GetService<ConfigService>();

    public TabManager<SampleTabModel> TabManager { get; } = new();
    public ReactiveCommand<Unit> DisposeCommand { get; }

    public Main_Model()
    {
        var baseType = typeof(SampleTabModel);
        foreach (var type in GetType().Assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface || type.IsAssignableTo(baseType) is false)
                continue;

            TabManager.Open(() => Service.CreateInstance<SampleTabModel>(type), _ => false);
        }

        TabManager.SelectedItem = TabManager.Items.FirstOrDefault(x => string.Equals(x.Header, _configService.Config.SelectedSample, StringComparison.Ordinal)) ?? TabManager.Items.FirstOrDefault();

        DisposeCommand = new(_ =>
        {
            if (Design.IsDesignMode)
                return;

            TabManager.Dispose();

            this.GetWindow().Content = null;

            for (var i = 0; i < 100; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        });
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
            _configService.Config.SelectedSample = TabManager.SelectedItem?.Header;
            _configService.Config.MainWindow.WindowState = window.WindowState;
            _configService.Config.MainWindow.Left = window.Position.X;
            _configService.Config.MainWindow.Top = window.Position.Y;

            if (window.WindowState == WindowState.Normal)
            {
                _configService.Config.MainWindow.Width = window.Width;
                _configService.Config.MainWindow.Height = window.Height;
            }

            DisposeCommand.Execute(Unit.Default);
        };
    }
}
