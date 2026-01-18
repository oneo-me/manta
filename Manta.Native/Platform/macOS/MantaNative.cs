using System.Runtime.InteropServices;

namespace Manta.Native.Platform.macOS;

/// <summary>
///     Native library wrapper for macOS WebView and Tray implementation.
/// </summary>
static partial class MantaNative
{
    const string LibraryName = "libMantaNative";

    #region WebView

    /// <summary>
    ///     Creates a new WebView instance and returns its handle.
    /// </summary>
    [LibraryImport(LibraryName, EntryPoint = "manta_webview_create")]
    public static partial long CreateWebView();

    /// <summary>
    ///     Destroys a WebView instance by its handle.
    /// </summary>
    [LibraryImport(LibraryName, EntryPoint = "manta_webview_destroy")]
    public static partial void DestroyWebView(long handle);

    /// <summary>
    ///     Gets the native NSView pointer for the WebView.
    /// </summary>
    [LibraryImport(LibraryName, EntryPoint = "manta_webview_get_native_handle")]
    public static partial IntPtr GetNativeHandle(long handle);

    /// <summary>
    ///     Loads a URL in the WebView.
    /// </summary>
    [LibraryImport(LibraryName, EntryPoint = "manta_webview_load_url", StringMarshalling = StringMarshalling.Utf8)]
    public static partial void LoadUrl(long handle, string url);

    /// <summary>
    ///     Loads HTML content in the WebView.
    /// </summary>
    [LibraryImport(LibraryName, EntryPoint = "manta_webview_load_html", StringMarshalling = StringMarshalling.Utf8)]
    public static partial void LoadHtml(long handle, string html, string? baseUrl);

    /// <summary>
    ///     Sets the frame (position and size) of the WebView.
    /// </summary>
    [LibraryImport(LibraryName, EntryPoint = "manta_webview_set_frame")]
    public static partial void SetFrame(long handle, double x, double y, double width, double height);

    #endregion

    #region Tray

    /// <summary>
    ///     Delegate for tray click callback.
    /// </summary>
    /// <param name="x">X coordinate of the center-bottom of the tray icon (Avalonia coordinates).</param>
    /// <param name="y">Y coordinate of the center-bottom of the tray icon (Avalonia coordinates).</param>
    /// <param name="screenX">X origin of the screen containing the tray icon.</param>
    /// <param name="screenY">Y origin of the screen containing the tray icon.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void TrayClickCallback(double x, double y, double screenX, double screenY);

    /// <summary>
    ///     Creates a new Tray instance and returns its handle.
    /// </summary>
    /// <param name="iconData">Pointer to icon PNG data.</param>
    /// <param name="iconLength">Length of icon data.</param>
    /// <param name="isTemplate">Whether to treat the icon as a template image (true for monochrome icons).</param>
    /// <param name="callback">Click callback function pointer.</param>
    /// <returns>Handle to the created tray instance.</returns>
    [LibraryImport(LibraryName, EntryPoint = "manta_tray_create")]
    public static unsafe partial long CreateTray
    (
        byte* iconData,
        int iconLength,
        [MarshalAs(UnmanagedType.Bool)] bool isTemplate,
        TrayClickCallback? callback
    );

    /// <summary>
    ///     Destroys a Tray instance by its handle.
    /// </summary>
    [LibraryImport(LibraryName, EntryPoint = "manta_tray_destroy")]
    public static partial void DestroyTray(long handle);

    /// <summary>
    ///     Sets the click callback for a Tray instance.
    /// </summary>
    [LibraryImport(LibraryName, EntryPoint = "manta_tray_set_callback")]
    public static partial void SetTrayCallback(long handle, TrayClickCallback? callback);

    #endregion

    #region Window Level/Type

    /// <summary>
    ///     Sets the window level for an NSWindow.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    /// <param name="level">Window level value.</param>
    [LibraryImport(LibraryName, EntryPoint = "manta_window_set_level")]
    public static partial void SetWindowLevel(IntPtr windowHandle, int level);

    /// <summary>
    ///     Sets the window collection behavior.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    /// <param name="behavior">Collection behavior flags.</param>
    [LibraryImport(LibraryName, EntryPoint = "manta_window_set_collection_behavior")]
    public static partial void SetWindowCollectionBehavior(IntPtr windowHandle, uint behavior);

    /// <summary>
    ///     Sets the window style mask.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    /// <param name="styleMask">Style mask value.</param>
    [LibraryImport(LibraryName, EntryPoint = "manta_window_set_style_mask")]
    public static partial void SetWindowStyleMask(IntPtr windowHandle, uint styleMask);

    /// <summary>
    ///     Gets the current window level.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    /// <returns>Current window level.</returns>
    [LibraryImport(LibraryName, EntryPoint = "manta_window_get_level")]
    public static partial int GetWindowLevel(IntPtr windowHandle);

    /// <summary>
    ///     Checks if the current appearance is dark mode.
    /// </summary>
    /// <returns>True if dark mode, false otherwise.</returns>
    [LibraryImport(LibraryName, EntryPoint = "manta_is_dark_mode")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool IsDarkMode();

    #endregion

    #region Blur/Visual Effect State

    /// <summary>
    ///     Sets the NSVisualEffectView state to keep blur active even when window loses focus.
    ///     This prevents the blur from dimming when the window is not active.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    /// <param name="alwaysActive">If true, blur stays active even when window loses focus.</param>
    [LibraryImport(LibraryName, EntryPoint = "manta_window_set_blur_always_active")]
    public static partial void SetWindowBlurAlwaysActive(IntPtr windowHandle, [MarshalAs(UnmanagedType.Bool)] bool alwaysActive);

    #endregion

    #region Non-Activating Panel

    /// <summary>
    ///     Shows the window without activating the application.
    ///     Useful for tray popup windows that should not steal focus.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    [LibraryImport(LibraryName, EntryPoint = "manta_window_order_front_regardless")]
    public static partial void OrderFrontRegardless(IntPtr windowHandle);

    /// <summary>
    ///     Sets whether the window can become the key window.
    ///     Set to false for windows that should not receive keyboard focus.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    /// <param name="canBecomeKey">Whether the window can become key window.</param>
    [LibraryImport(LibraryName, EntryPoint = "manta_window_set_can_become_key")]
    public static partial void SetWindowCanBecomeKey(IntPtr windowHandle, [MarshalAs(UnmanagedType.Bool)] bool canBecomeKey);

    /// <summary>
    ///     Configures the window as a non-activating panel.
    ///     This is a convenience function that combines multiple settings for tray-style popups.
    /// </summary>
    /// <param name="windowHandle">Native NSWindow pointer.</param>
    /// <param name="isNonActivating">If true, window will not activate the app when shown.</param>
    [LibraryImport(LibraryName, EntryPoint = "manta_window_set_non_activating")]
    public static partial void SetWindowNonActivating(IntPtr windowHandle, [MarshalAs(UnmanagedType.Bool)] bool isNonActivating);

    #endregion
}
