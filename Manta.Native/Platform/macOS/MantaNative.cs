using System.Runtime.InteropServices;

namespace Manta.Native.Platform.macOS;

/// <summary>
///     Native library wrapper for macOS WebView implementation.
/// </summary>
static partial class MantaNative
{
    const string LibraryName = "libMantaNative";

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
}
