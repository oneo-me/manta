using Avalonia.Controls;

namespace AirKit.Reactive.Controls;

public static class LightWindowExtensions
{
    public static void FocusActivate(this Window window)
    {
        var oldTopmost = window.Topmost;
        window.Topmost = true;
        window.Activate();
        window.Topmost = oldTopmost;
    }
}
