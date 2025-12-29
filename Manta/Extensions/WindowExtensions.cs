using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;

namespace Manta.Extensions;

public static class WindowExtensions
{
    extension(Window window)
    {
        public void FocusActivate()
        {
            var oldTopmost = window.Topmost;
            window.Topmost = true;
            window.Activate();
            window.Topmost = oldTopmost;
        }

        public void KeepCentered()
        {
            window.Resized += (_, e) =>
            {
                if (e.Reason != WindowResizeReason.User)
                    window.Centered();
            };
        }

        public void Centered()
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

        public IObservable<WindowStatus> GetWindowStatusObservable()
        {
            if (Design.IsDesignMode)
                return Observable.Empty<WindowStatus>();

            return window.GetObservable(Window.WindowStateProperty)
                .StartWith(window.WindowState)
                .Select(x =>
                {
                    var isMacos = OperatingSystem.IsMacOS();
                    var isShowButtons = x is not WindowState.FullScreen;

                    return new WindowStatus
                    {
                        IsFullScreen = x is WindowState.FullScreen,
                        IsShowButtons = isShowButtons && isMacos is false,
                        Offset = new(isMacos ? isShowButtons ? 78 : 0 : 0, isMacos ? isShowButtons ? 38 : 0 : 0, 0, 0)
                    };
                });
        }
    }
}

public struct WindowStatus
{
    public bool IsFullScreen { get; set; }
    public bool IsShowButtons { get; set; }
    public Thickness Offset { get; set; }
}
