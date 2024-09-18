using AirKit.Reactive.Attributes;
using AirKit.Reactive.Extensions;
using Avalonia.Controls;
using R3;

namespace AirKit.Reactive.Demo.Views;

[View<Main>]
public class Main_Model : WindowModel
{
    public ReactiveCommand<Unit> DisposeCommand { get; }

    public Main_Model()
    {
        DisposeCommand = new(_ =>
        {
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
        window.Title = "Reactive Demo";
        window.Width = 560;
        window.Height = 500;
    }
}
