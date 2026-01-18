using Avalonia;

namespace Manta.Native.Platform.macOS;

/// <summary>
///     Event args for tray click events, includes screen information.
/// </summary>
sealed class TrayClickEventArgs(Point position, Point screenOrigin) : EventArgs
{
    /// <summary>
    ///     Position of the center-bottom of the tray icon in Avalonia coordinates.
    /// </summary>
    public Point Position { get; } = position;

    /// <summary>
    ///     Origin of the screen containing the tray icon.
    /// </summary>
    public Point ScreenOrigin { get; } = screenOrigin;
}
