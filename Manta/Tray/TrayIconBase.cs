using Avalonia;
using Manta.Native.Platform.macOS;

namespace Manta.Tray;

abstract class TrayIconBase : IDisposable
{
    public event EventHandler<TrayClickEventArgs>? Clicked;

    protected void OnClicked(Point position, Point screenOrigin)
    {
        Clicked?.Invoke(this, new(position, screenOrigin));
    }

    public abstract void Dispose();
}
