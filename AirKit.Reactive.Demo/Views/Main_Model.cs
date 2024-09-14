using AirKit.Reactive.Attributes;
using AirKit.Reactive.Extensions;
using Avalonia.Controls;

namespace AirKit.Reactive.Demo.Views;

[View<Main>]
public class Main_Model : WindowModel
{
    public override void OnWindowInitialized(Window window)
    {
        window.Title = "Reactive Demo";
        window.Width = 560;
        window.Height = 500;
        window.Content = new DemoView_Model().CreateView();
    }
}
