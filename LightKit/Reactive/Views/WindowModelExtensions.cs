using Avalonia.Controls;
using LightKit.Controls;

namespace LightKit.Reactive.Views;

public static class WindowModelExtensions
{
    public static Window CreateWindow(this WindowModel windowModel)
    {
        var result = new LightWindow
        {
            Content = windowModel.CreateView()
        };
        windowModel.InnerWindow = result;

        // Set window properties
        if (LightApp.Current.Icon is not null)
            result.Icon = new(LightApp.Current.Icon);
        result.Title = LightApp.Current.Name;

        // Activate window on open
        result.Opened += (sender, _) =>
        {
            if (sender is Window current)
            {
                current.FocusActivate();
                current.Centered();
            }
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
