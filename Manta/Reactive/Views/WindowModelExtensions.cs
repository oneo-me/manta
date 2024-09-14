using Avalonia.Controls;
using Manta.Extensions;
using Manta.Controls;

namespace Manta.Reactive.Views;

public static class WindowModelExtensions
{
    extension(WindowModel windowModel)
    {
        public Window CreateWindow()
        {
            var result = new MWindow
            {
                Content = windowModel.CreateView()
            };
            windowModel.InnerWindow = result;

            // Set window properties
            if (MApp.Current.Icon is not null)
                result.Icon = new(MApp.Current.Icon);
            result.Title = MApp.Current.Name;

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
