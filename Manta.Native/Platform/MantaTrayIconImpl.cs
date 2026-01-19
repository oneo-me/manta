using Avalonia;

namespace Manta.Native.Platform;

abstract class MantaTrayIconImpl
{
    public event EventHandler<Point>? Clicked;

    protected void OnClicked(Point position)
    {
        Clicked?.Invoke(this, position);
    }

    public abstract void Dispose();
}
