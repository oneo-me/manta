using Avalonia.Controls;

namespace AirKit.Extensions;

public static class WindowExtensions
{
    public static void FocusActivate(this Window window)
    {
        var oldTopmost = window.Topmost;
        window.Topmost = true;
        window.Activate();
        window.Topmost = oldTopmost;
    }
}
