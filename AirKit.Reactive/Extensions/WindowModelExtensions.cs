using AirKit.Reactive.Controls;
using Avalonia.Controls;

namespace AirKit.Reactive.Extensions;

public static class WindowModelExtensions
{
    public static Window CreateWindow(this WindowModel windowModel)
    {
        var result = new LightWindow
        {
            Content = windowModel.CreateView()
        };

        windowModel.OnWindowInitialized(result);

        return result;
    }
}
