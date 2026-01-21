using Avalonia.Controls;
using Manta.Controls;
using Manta.Extensions;

namespace Manta.Reactive.Views;

public static class WindowModelExtensions
{
    static T ShowCore<T>(T windowModel, Window? owner = null) where T : WindowModel
    {
        var window = windowModel.CreateWindow();
        if (owner is null)
            window.Show();
        else
            window.Show(owner);
        return windowModel;
    }

    static T ShowDialogCore<T>(T windowModel, Window owner) where T : WindowModel
    {
        var window = windowModel.CreateWindow();
        window.ShowDialog(owner);
        return windowModel;
    }

    extension<T>(T windowModel) where T : WindowModel
    {
        public async Task<T> ShowAsync(Window? owner = null)
        {
            if (UIThread.CheckAccess())
                return ShowCore(windowModel, owner);

            return await UIThread.InvokeAsync(() => ShowCore(windowModel, owner));
        }

        public async Task<T> ShowDialogAsync(Window owner)
        {
            if (UIThread.CheckAccess())
                return ShowDialogCore(windowModel, owner);

            return await UIThread.InvokeAsync(() => ShowDialogCore(windowModel, owner));
        }
    }

    extension(WindowModel windowModel)
    {
        internal MWindow CreateWindow()
        {
            if (windowModel.InnerWindow is not null)
                return windowModel.InnerWindow;

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

        public MWindow GetWindow()
        {
            if (windowModel.InnerWindow is null)
                throw new InvalidOperationException($"Window not found for {windowModel}");

            return windowModel.InnerWindow;
        }
    }
}
