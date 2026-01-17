using Avalonia;

namespace Manta.Tray;

/// <summary>
///     Event args for tray click events.
/// </summary>
sealed class TaryClickEventArgs(Point position, Point screenOrigin) : EventArgs
{
    /// <summary>
    ///     Position of the center-bottom of the tray icon.
    /// </summary>
    public Point Position { get; } = position;

    /// <summary>
    ///     Origin of the screen containing the tray icon (for multi-monitor support).
    /// </summary>
    public Point ScreenOrigin { get; } = screenOrigin;
}

abstract class TaryIconBase : IDisposable
{
    public event EventHandler<TaryClickEventArgs>? Clicked;

    protected void OnClicked(Point position, Point screenOrigin)
    {
        Clicked?.Invoke(this, new(position, screenOrigin));
    }

    public abstract void Dispose();
}
