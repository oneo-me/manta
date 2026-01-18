namespace Manta.Native.Platform.macOS;

/// <summary>
///     Wrapper for macOS WKWebView using the MantaNative Swift library.
/// </summary>
sealed class NativeWebViewWrapper : IDisposable
{
    readonly long _handle;
    bool _disposed;

    public NativeWebViewWrapper()
    {
        _handle = MantaNative.CreateWebView();

        if (_handle <= 0)
            throw new InvalidOperationException("Failed to create WebView - invalid handle");
    }

    /// <summary>
    ///     Gets the native NSView handle for embedding in Avalonia.
    /// </summary>
    public IntPtr Handle
    {
        get
        {
            if (_disposed) return IntPtr.Zero;
            return MantaNative.GetNativeHandle(_handle);
        }
    }

    /// <summary>
    ///     Loads a URL in the WebView.
    /// </summary>
    public void LoadUrl(string url)
    {
        if (_disposed || string.IsNullOrEmpty(url))
            return;

        MantaNative.LoadUrl(_handle, url);
    }

    /// <summary>
    ///     Loads HTML content in the WebView.
    /// </summary>
    public void LoadHtmlString(string html, string? baseUrl = null)
    {
        if (_disposed || string.IsNullOrEmpty(html))
            return;

        MantaNative.LoadHtml(_handle, html, baseUrl);
    }

    /// <summary>
    ///     Sets the frame (position and size) of the WebView.
    /// </summary>
    public void SetFrame(double x, double y, double width, double height)
    {
        if (_disposed) return;
        MantaNative.SetFrame(_handle, x, y, width, height);
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        MantaNative.DestroyWebView(_handle);

        GC.SuppressFinalize(this);
    }

    ~NativeWebViewWrapper()
    {
        Dispose();
    }
}
