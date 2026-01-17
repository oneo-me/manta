using System.Diagnostics.CodeAnalysis;
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

/// <summary>
///     Wrapper for macOS NSStatusItem using the MantaNative Swift library.
/// </summary>
sealed class NativeTrayWrapper : IDisposable
{
    readonly long _handle;
    bool _disposed;

    // Keep a reference to the callback to prevent GC collection
    MantaNative.TrayClickCallback? _callback;

    /// <summary>
    ///     Event raised when the tray icon is clicked.
    /// </summary>
    public event EventHandler<TrayClickEventArgs>? Clicked;

    /// <summary>
    ///     Creates a new native tray icon.
    /// </summary>
    /// <param name="icon">PNG data for icon.</param>
    /// <param name="isTemplate">Whether to treat the icon as a template image (true for monochrome icons that should adapt to system theme).</param>
    public unsafe NativeTrayWrapper(byte[] icon, bool isTemplate = true)
    {
        if (icon == null || icon.Length == 0)
            throw new ArgumentException("Icon data cannot be null or empty.", nameof(icon));

        _callback = OnTrayClicked;

        fixed (byte* iconPtr = icon)
        {
            _handle = MantaNative.CreateTray(
                iconPtr,
                icon.Length,
                isTemplate,
                _callback);
        }

        if (_handle <= 0)
            throw new InvalidOperationException("Failed to create tray icon - invalid handle");
    }

    void OnTrayClicked(double x, double y, double screenX, double screenY)
    {
        Clicked?.Invoke(this, new TrayClickEventArgs(new Point(x, y), new Point(screenX, screenY)));
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        MantaNative.DestroyTray(_handle);
        _callback = null;

        GC.SuppressFinalize(this);
    }

    ~NativeTrayWrapper()
    {
        Dispose();
    }
}

/// <summary>
///     macOS window level constants.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MacOSWindowLevel
{
    Normal = 0,
    Floating = 3,
    Submenu = 4,
    TornOffMenu = 5,
    MainMenu = 11,
    Status = 12,
    ModalPanel = 13,
    PopUpMenu = 101,
    ScreenSaver = 1000
}

/// <summary>
///     macOS window collection behavior flags.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
[Flags]
public enum MacOSWindowCollectionBehavior : uint
{
    Default = 0,
    CanJoinAllSpaces = 1 << 0,
    MoveToActiveSpace = 1 << 1,
    Managed = 1 << 2,
    Transient = 1 << 3,
    Stationary = 1 << 4,
    ParticipatesInCycle = 1 << 5,
    IgnoresCycle = 1 << 6,
    FullScreenPrimary = 1 << 7,
    FullScreenAuxiliary = 1 << 8,
    FullScreenNone = 1 << 9,
    FullScreenAllowsTiling = 1 << 11,
    FullScreenDisallowsTiling = 1 << 12
}

/// <summary>
///     Helper class for macOS window operations.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MacOSWindowHelper
{
    /// <summary>
    ///     Sets the window level for an NSWindow.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    /// <param name="level">Window level.</param>
    public static void SetWindowLevel(IntPtr windowHandle, MacOSWindowLevel level)
    {
        if (windowHandle == IntPtr.Zero) return;
        MantaNative.SetWindowLevel(windowHandle, (int)level);
    }

    /// <summary>
    ///     Sets the window collection behavior.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    /// <param name="behavior">Collection behavior flags.</param>
    public static void SetWindowCollectionBehavior(IntPtr windowHandle, MacOSWindowCollectionBehavior behavior)
    {
        if (windowHandle == IntPtr.Zero) return;
        MantaNative.SetWindowCollectionBehavior(windowHandle, (uint)behavior);
    }

    /// <summary>
    ///     Sets the window style mask.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    /// <param name="styleMask">Style mask value.</param>
    public static void SetWindowStyleMask(IntPtr windowHandle, uint styleMask)
    {
        if (windowHandle == IntPtr.Zero) return;
        MantaNative.SetWindowStyleMask(windowHandle, styleMask);
    }

    /// <summary>
    ///     Gets the current window level.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    /// <returns>Current window level.</returns>
    public static MacOSWindowLevel GetWindowLevel(IntPtr windowHandle)
    {
        if (windowHandle == IntPtr.Zero) return MacOSWindowLevel.Normal;
        return (MacOSWindowLevel)MantaNative.GetWindowLevel(windowHandle);
    }

    /// <summary>
    ///     Checks if the current appearance is dark mode.
    /// </summary>
    /// <returns>True if dark mode, false otherwise.</returns>
    public static bool IsDarkMode()
    {
        return MantaNative.IsDarkMode();
    }
}
