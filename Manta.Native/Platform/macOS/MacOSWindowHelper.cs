using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;

namespace Manta.Native.Platform.macOS;

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

    public static void ConfigureAsWindow(Window trayWindow)
    {
        if (!OperatingSystem.IsMacOS()) return;

        var handle = GetNativeWindowHandle(trayWindow);
        if (handle == IntPtr.Zero) return;

        // Stay blurry and always active
        MantaNative.SetWindowBlurAlwaysActive(handle, true);
    }

    public static void ConfigureAsTrayWindow(Window trayWindow)
    {
        if (!OperatingSystem.IsMacOS()) return;

        var handle = GetNativeWindowHandle(trayWindow);
        if (handle == IntPtr.Zero) return;

        // Stay blurry and always active
        MantaNative.SetWindowBlurAlwaysActive(handle, true);

        // Set window level to PopUpMenu (above most windows)
        SetWindowLevel(handle, MacOSWindowLevel.PopUpMenu);

        // Set collection behavior: transient, ignores cycle
        SetWindowCollectionBehavior(handle,
            MacOSWindowCollectionBehavior.Transient |
            MacOSWindowCollectionBehavior.IgnoresCycle |
            MacOSWindowCollectionBehavior.MoveToActiveSpace);

        // Set as inactive window
        MantaNative.SetWindowNonActivating(handle, true);
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
