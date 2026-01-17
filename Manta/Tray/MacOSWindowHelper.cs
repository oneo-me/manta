using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Manta.Native.Platform.macOS;

namespace Manta.Tray;

/// <summary>
///     macOS window level constants.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MacOSTrayWindowLevel
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
[Flags]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum MacOSTrayWindowCollectionBehavior : uint
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
///     Helper class for macOS-specific window operations, primarily for tray windows.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MacOSTrayWindowHelper
{
    /// <summary>
    ///     Sets the window level for a Window.
    /// </summary>
    /// <param name="window">The Avalonia window.</param>
    /// <param name="level">Window level.</param>
    public static void SetWindowLevel(Window window, MacOSTrayWindowLevel level)
    {
        if (!OperatingSystem.IsMacOS()) return;

        var handle = GetNativeWindowHandle(window);
        if (handle == IntPtr.Zero) return;

        MacOSWindowHelper.SetWindowLevel(handle, (MacOSWindowLevel)(int)level);
    }

    /// <summary>
    ///     Sets the window collection behavior.
    /// </summary>
    /// <param name="window">The Avalonia window.</param>
    /// <param name="behavior">Collection behavior flags.</param>
    public static void SetWindowCollectionBehavior(Window window, MacOSTrayWindowCollectionBehavior behavior)
    {
        if (!OperatingSystem.IsMacOS()) return;

        var handle = GetNativeWindowHandle(window);
        if (handle == IntPtr.Zero) return;

        MacOSWindowHelper.SetWindowCollectionBehavior(handle, (MacOSWindowCollectionBehavior)(uint)behavior);
    }

    /// <summary>
    ///     Gets the current window level.
    /// </summary>
    /// <param name="window">The Avalonia window.</param>
    /// <returns>Current window level.</returns>
    public static MacOSTrayWindowLevel GetWindowLevel(Window window)
    {
        if (!OperatingSystem.IsMacOS()) return MacOSTrayWindowLevel.Normal;

        var handle = GetNativeWindowHandle(window);
        if (handle == IntPtr.Zero) return MacOSTrayWindowLevel.Normal;

        return (MacOSTrayWindowLevel)(int)MacOSWindowHelper.GetWindowLevel(handle);
    }

    /// <summary>
    ///     Configures a window as a tray popup window with appropriate level and behavior.
    /// </summary>
    /// <param name="window">The Avalonia window to configure.</param>
    public static void ConfigureAsTrayWindow(Window window)
    {
        if (!OperatingSystem.IsMacOS()) return;

        var handle = GetNativeWindowHandle(window);
        if (handle == IntPtr.Zero) return;

        // Set window level to PopUpMenu (above most windows)
        MacOSWindowHelper.SetWindowLevel(handle, MacOSWindowLevel.PopUpMenu);

        // Set collection behavior: transient, ignores cycle
        MacOSWindowHelper.SetWindowCollectionBehavior(handle,
            MacOSWindowCollectionBehavior.Transient |
            MacOSWindowCollectionBehavior.IgnoresCycle |
            MacOSWindowCollectionBehavior.MoveToActiveSpace);
    }

    /// <summary>
    ///     Checks if the current system appearance is dark mode.
    /// </summary>
    /// <returns>True if dark mode, false otherwise.</returns>
    public static bool IsDarkMode()
    {
        if (!OperatingSystem.IsMacOS()) return false;
        return MacOSWindowHelper.IsDarkMode();
    }

    static IntPtr GetNativeWindowHandle(Window window)
    {
        try
        {
            var platformHandle = window.TryGetPlatformHandle();
            return platformHandle?.Handle ?? IntPtr.Zero;
        }
        catch
        {
            return IntPtr.Zero;
        }
    }
}
