using Avalonia.Controls;

namespace LightKit.Reactive.Views;

public static class WindowExtensions
{
    public static void FocusActivate(this Window window)
    {
        var oldTopmost = window.Topmost;
        window.Topmost = true;
        window.Activate();
        window.Topmost = oldTopmost;
    }

    public static void KeepCentered(this Window window)
    {
        window.Resized += (_, e) =>
        {
            if (e.Reason != WindowResizeReason.User)
                window.Centered();
        };
    }

    public static void Centered(this Window window)
    {
        var scale = window.DesktopScaling;

        // 保持在拥有者窗口的中间偏上位置
        if (window.Owner is Window ownerWindow)
        {
            var newLeft = ownerWindow.Position.X + (ownerWindow.ClientSize.Width * scale - window.ClientSize.Width * scale) / 2;
            var newTop = ownerWindow.Position.Y + (ownerWindow.ClientSize.Height * scale - window.ClientSize.Height * scale) / 2.5;
            window.Position = new((int)newLeft, (int)newTop);

            return;
        }

        // 保持在屏幕正中间
        var screen = window.Screens.ScreenFromWindow(window);
        if (screen is not null)
        {
            var workingArea = screen.WorkingArea;
            var newLeft = workingArea.X + (workingArea.Width - window.ClientSize.Width * scale) / 2;
            var newTop = workingArea.Y + (workingArea.Height - window.ClientSize.Height * scale) / 2;
            window.Position = new((int)newLeft, (int)newTop);
        }
    }
}
