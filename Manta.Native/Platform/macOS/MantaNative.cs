using System.Runtime.InteropServices;

namespace Manta.Native.Platform.macOS;

/// <summary>
///     Native library wrapper for macOS WebView and Tray implementation.
/// </summary>
static partial class MantaNative
{
    const string LibraryName = "libMantaNative";

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
}
