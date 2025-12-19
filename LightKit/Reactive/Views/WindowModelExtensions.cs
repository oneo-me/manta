using Avalonia.Controls;
using LightKit.Controls;
using LightKit.Extensions;

namespace LightKit.Reactive.Views;

public static class WindowModelExtensions
{
    extension(WindowModel windowModel)
    {
        public Window CreateWindow()
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

        public Window GetWindow()
        {
            if (windowModel.InnerWindow is null)
                throw new InvalidOperationException($"Window not found for {windowModel}");

            return windowModel.InnerWindow;
        }
    }
}
