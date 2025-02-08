using Avalonia.Controls;

namespace AirKit.Extensions;

public static class WindowModelExtensions
{
    public static Window CreateWindow(this WindowModel windowModel)
    {
        var result = new Window
        {
            Content = windowModel.CreateView()
        };
        windowModel.InnerWindow = result;

        // Set window properties
        if (DesktopApp.Current.Icon is not null)
            result.Icon = new(DesktopApp.Current.Icon);
        result.Title = DesktopApp.Current.Name;

        // Activate window on open
        result.Opened += (sender, _) =>
        {
            if (sender is Window current)
                current.FocusActivate();
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
