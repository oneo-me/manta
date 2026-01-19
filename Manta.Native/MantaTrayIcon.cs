using Avalonia;
using Manta.Native.Platform;
using Manta.Native.Platform.macOS;
using Manta.Native.Platform.Windows;

namespace Manta.Native;

public sealed class MantaTrayIcon : IDisposable
{
    MantaTrayIconImpl? _impl;

    public event EventHandler<Point>? Clicked;

    public MantaTrayIcon(Application app, byte[] taryIcon)
    {
        if (OperatingSystem.IsMacOS())
            _impl = new MacOSTrayIcon(taryIcon);
        else if (OperatingSystem.IsWindows())
            _impl = new WindowsTrayIcon(app, taryIcon);

        if (_impl is null)
            throw new PlatformNotSupportedException("Tray icon is not supported on this platform.");

        _impl.Clicked += ImplOnClicked;
    }

    void ImplOnClicked(object? sender, Point e)
    {
        Clicked?.Invoke(this, e);
    }

    public void Dispose()
    {
        if (_impl is null)
            return;

        _impl.Clicked -= ImplOnClicked;
        _impl.Dispose();
        _impl = null;
    }
}
