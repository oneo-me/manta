using AirKit.Reactive.Attributes;
using AirKit.Reactive.Demo.Models;
using AirKit.Reactive.Extensions;
using Avalonia.Controls;
using R3;

namespace AirKit.Reactive.Demo.Views;

[View<Main>]
sealed class Main_Model : WindowModel
{
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

        TabManager.SelectedItem = TabManager.Items.FirstOrDefault();

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

        window.Title = "Reactive Demo";
        window.Width = 720;
        window.Height = 660;

        window.Closing += (_, _) => DisposeCommand.Execute(Unit.Default);
    }
}
