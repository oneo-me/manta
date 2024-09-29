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

    public static Window GetWindow(this WindowModel windowModel)
    {
        if (windowModel.InnerWindow is null)
            throw new InvalidOperationException($"Window not found for {windowModel}");

        return windowModel.InnerWindow;
    }
}
