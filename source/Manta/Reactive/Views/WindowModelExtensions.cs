using Avalonia.Controls;
using Manta.Controls;
using Manta.Extensions;

namespace Manta.Reactive.Views;

public static class WindowModelExtensions
{
    internal static MWindow GetWindowCore(this WindowModel windowModel)
    {
        if (windowModel.InnerWindow is null)
            throw new InvalidOperationException($"Window not found for {windowModel}");

        return windowModel.InnerWindow;
    }

    static T ShowCore<T>(T windowModel, Window? owner = null) where T : WindowModel
    {
        var window = windowModel.CreateWindow();
        if (owner is null)
            window.Show();
        else
            window.Show(owner);
        return windowModel;
    }

    static async Task<T> ShowDialogCoreAsync<T>(T windowModel, Window owner) where T : WindowModel
    {
        var window = windowModel.CreateWindow();
        await window.ShowDialog(owner).ConfigureAwait(false);
        return windowModel;
    }

    public static async Task<T> ShowAsync<T>(this T windowModel, ViewModel? owner = null) where T : WindowModel
    {
        var ownerWindow = owner is null ? null : await owner.GetWindowAsync().ConfigureAwait(false);

        if (UIThread.CheckAccess())
            return ShowCore(windowModel, ownerWindow);

        return await UIThread.InvokeAsync(() => ShowCore(windowModel, ownerWindow));
    }

    public static async Task<T> ShowDialogAsync<T>(this T windowModel, ViewModel owner) where T : WindowModel
    {
        var ownerWindow = await owner.GetWindowAsync().ConfigureAwait(false);

        if (UIThread.CheckAccess())
            return await ShowDialogCoreAsync(windowModel, ownerWindow).ConfigureAwait(false);

        return await UIThread.InvokeAsync(() => ShowDialogCoreAsync(windowModel, ownerWindow)).ConfigureAwait(false);
    }

    internal static MWindow CreateWindow(this WindowModel windowModel)
    {
        if (windowModel.InnerWindow is not null)
            return windowModel.InnerWindow;

        var result = new MWindow
        {
            Content = windowModel.BuildView()
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
}
