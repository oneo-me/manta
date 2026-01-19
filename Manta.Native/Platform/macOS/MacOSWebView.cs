using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Manta.Native.Platform.macOS;

[SuppressMessage("ReSharper", "InconsistentNaming")]
sealed partial class MacOSWebView : MantaWebView
{
    const string LibraryName = "libMantaNative";

    [LibraryImport(LibraryName, EntryPoint = "manta_webview_create")]
    private static partial IntPtr CreateWebView();

    [LibraryImport(LibraryName, EntryPoint = "manta_webview_destroy")]
    private static partial void DestroyWebView(IntPtr handle);

    [LibraryImport(LibraryName, EntryPoint = "manta_webview_load_url", StringMarshalling = StringMarshalling.Utf8)]
    private static partial void LoadUrl(IntPtr handle, string url);

    [LibraryImport(LibraryName, EntryPoint = "manta_webview_load_html", StringMarshalling = StringMarshalling.Utf8)]
    private static partial void LoadHtml(IntPtr handle, string html, string? baseUrl);

    public override IntPtr Handle { get; }

    public MacOSWebView()
    {
        Handle = CreateWebView();
        if (Handle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create MacOS WebView.");
    }

    public override void LoadUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return;

        LoadUrl(Handle, url);
    }

    public override void LoadHtmlString(string html, string? baseUrl = null)
    {
        if (string.IsNullOrEmpty(html))
            return;

        LoadHtml(Handle, html, baseUrl);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        DestroyWebView(Handle);
    }
}
