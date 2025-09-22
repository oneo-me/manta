using Avalonia.Controls;
using Axe.Reactive.Controls;

namespace Axe.Reactive;

public static class WindowModelExtensions
{
    public static Window CreateWindow(this WindowModel windowModel)
    {
        var result = new ReactiveWindow
        {
            Content = windowModel.CreateView()
        };
        windowModel.InnerWindow = result;

        // Set window properties
        if (AxeApp.Current.Icon is not null)
            result.Icon = new(AxeApp.Current.Icon);
        result.Title = AxeApp.Current.Name;

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
