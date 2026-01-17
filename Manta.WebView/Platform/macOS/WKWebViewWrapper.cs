using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Manta.Logging;

namespace Manta.WebView.Platform.macOS;

/// <summary>
///     Wrapper for macOS WKWebView using PInvoke to Objective-C runtime
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
sealed class WKWebViewWrapper : IDisposable
{
    IntPtr _webKitHandle;
    IntPtr _configuration;
    IntPtr _preferences;
    IntPtr _userContentController;
    bool _disposed;

    public IntPtr Handle { get; private set; }

    public WKWebViewWrapper()
    {
        CreateWebView();
    }

    void CreateWebView()
    {
        // Load WebKit framework
        _webKitHandle = dlopen("/System/Library/Frameworks/WebKit.framework/WebKit", RTLD_LAZY);
        
        if (_webKitHandle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to load WebKit framework");
        
        Log.Debug($"WebKit framework loaded: {_webKitHandle != IntPtr.Zero}");

        var alloc = ObjCRuntime.GetSelector("alloc");
        var init = ObjCRuntime.GetSelector("init");

        // Create WKPreferences with optimized settings
        var prefsClass = ObjCRuntime.GetClass("WKPreferences");
        _preferences = ObjCRuntime.SendMessage(prefsClass, alloc);
        _preferences = ObjCRuntime.SendMessage(_preferences, init);

        if (_preferences != IntPtr.Zero)
        {
            // Enable JavaScript
            ObjCRuntime.SendVoid(_preferences, ObjCRuntime.GetSelector("setJavaScriptEnabled:"), 1);

            // Enable JavaScript can open windows automatically
            ObjCRuntime.SendVoid(_preferences, ObjCRuntime.GetSelector("setJavaScriptCanOpenWindowsAutomatically:"), 1);

            // Set minimum font size for better readability
            ObjCRuntime.SendDouble(_preferences, ObjCRuntime.GetSelector("setMinimumFontSize:"), 0.0);

            // Enable text interaction
            ObjCRuntime.SendVoid(_preferences, ObjCRuntime.GetSelector("setTextInteractionEnabled:"), 1);
        }

        // Create WKUserContentController for script injection
        var contentControllerClass = ObjCRuntime.GetClass("WKUserContentController");
        _userContentController = ObjCRuntime.SendMessage(contentControllerClass, alloc);
        _userContentController = ObjCRuntime.SendMessage(_userContentController, init);

        // Create WKWebViewConfiguration
        var configClass = ObjCRuntime.GetClass("WKWebViewConfiguration");
        Log.Debug($"WKWebViewConfiguration class: {configClass}");

        if (configClass == IntPtr.Zero)
            throw new InvalidOperationException("Failed to get WKWebViewConfiguration class");

        _configuration = ObjCRuntime.SendMessage(configClass, alloc);
        _configuration = ObjCRuntime.SendMessage(_configuration, init);
        Log.Debug($"Configuration created: {_configuration}");

        if (_configuration == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create WKWebViewConfiguration");

        // Apply preferences to configuration
        if (_preferences != IntPtr.Zero)
            ObjCRuntime.SendVoid(_configuration, ObjCRuntime.GetSelector("setPreferences:"), _preferences);

        // Apply user content controller
        if (_userContentController != IntPtr.Zero)
            ObjCRuntime.SendVoid(_configuration, ObjCRuntime.GetSelector("setUserContentController:"), _userContentController);

        // Create WKWebView with frame
        var webViewClass = ObjCRuntime.GetClass("WKWebView");
        Log.Debug($"WKWebView class: {webViewClass}");

        if (webViewClass == IntPtr.Zero)
            throw new InvalidOperationException("Failed to get WKWebView class");

        var initWithFrame = ObjCRuntime.GetSelector("initWithFrame:configuration:");

        Handle = ObjCRuntime.SendMessage(webViewClass, alloc);
        Log.Debug($"WKWebView allocated: {Handle}");

        // Initialize with a non-zero frame
        var frame = new ObjCRuntime.CGRect(0, 0, 100, 100);
        Handle = SendMessageWithFrameAndConfig(Handle, initWithFrame, frame, _configuration);
        Log.Debug($"WKWebView initialized: {Handle}");

        if (Handle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to initialize WKWebView");

        // Verify it's an NSView
        var isNSView = ObjCRuntime.IsKindOfClass(Handle, "NSView");
        var isWKWebView = ObjCRuntime.IsKindOfClass(Handle, "WKWebView");
        Log.Debug($"Is NSView: {isNSView}, Is WKWebView: {isWKWebView}");

        // Set wantsLayer to YES for proper rendering in layer-backed views
        ObjCRuntime.SendVoid(Handle, ObjCRuntime.GetSelector("setWantsLayer:"), 1);

        // Enable layer-backed drawing for better performance
        var layer = ObjCRuntime.SendMessage(Handle, ObjCRuntime.GetSelector("layer"));
        if (layer != IntPtr.Zero)
            // Set layer to use GPU acceleration
            ObjCRuntime.SendVoid(layer, ObjCRuntime.GetSelector("setDrawsAsynchronously:"), 1);

        // Set custom User-Agent to ensure proper desktop rendering
        var userAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.0 Safari/605.1.15";
        var userAgentString = ObjCRuntime.CreateNSString(userAgent);
        ObjCRuntime.SendVoid(Handle, ObjCRuntime.GetSelector("setCustomUserAgent:"), userAgentString);
        ObjCRuntime.Release(userAgentString);

        // Allow magnification for better zoom support
        ObjCRuntime.SendVoid(Handle, ObjCRuntime.GetSelector("setAllowsMagnification:"), 1);

        // Set page zoom to 1.0 (100%)
        ObjCRuntime.SendDouble(Handle, ObjCRuntime.GetSelector("setPageZoom:"), 1.0);
    }

    [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    static extern IntPtr SendMessageWithFrameAndConfig
    (
        IntPtr receiver,
        IntPtr selector,
        ObjCRuntime.CGRect frame,
        IntPtr config
    );

    [DllImport("/usr/lib/libSystem.B.dylib")]
    static extern IntPtr dlopen(string path, int mode);

    [DllImport("/usr/lib/libSystem.B.dylib")]
    static extern int dlclose(IntPtr handle);

    // dlopen mode constants
    const int RTLD_LAZY = 0x1;

    public void LoadUrl(string url)
    {
        if (_disposed || string.IsNullOrEmpty(url))
        {
            Log.Debug($"LoadUrl skipped: disposed={_disposed}, url={url}");
            return;
        }

        Log.Debug($"LoadUrl: {url}");

        var nsUrl = ObjCRuntime.CreateNSURL(url);
        Log.Debug($"NSURL created: {nsUrl}");

        if (nsUrl == IntPtr.Zero)
        {
            Log.Debug("Failed to create NSURL");
            return;
        }

        try
        {
            var requestClass = ObjCRuntime.GetClass("NSURLRequest");
            var requestWithUrl = ObjCRuntime.GetSelector("requestWithURL:");
            var request = ObjCRuntime.SendMessage(requestClass, requestWithUrl, nsUrl);
            Log.Debug($"NSURLRequest created: {request}");

            if (request == IntPtr.Zero)
            {
                Log.Debug("Failed to create NSURLRequest");
                return;
            }

            var loadRequest = ObjCRuntime.GetSelector("loadRequest:");
            ObjCRuntime.SendVoid(Handle, loadRequest, request);
            Log.Debug("loadRequest sent");
        }
        finally
        {
            // Release the NSURL we created
            ObjCRuntime.Release(nsUrl);
        }
    }

    public void LoadHtmlString(string html, string? baseUrl = null)
    {
        if (_disposed || string.IsNullOrEmpty(html))
            return;

        var htmlString = ObjCRuntime.CreateNSString(html);
        var baseUrlObj = string.IsNullOrEmpty(baseUrl)
            ? IntPtr.Zero
            : ObjCRuntime.CreateNSURL(baseUrl);

        try
        {
            var loadHtml = ObjCRuntime.GetSelector("loadHTMLString:baseURL:");
            ObjCRuntime.SendVoid(Handle, loadHtml, htmlString, baseUrlObj);
        }
        finally
        {
            // Release temporary objects
            if (htmlString != IntPtr.Zero)
                ObjCRuntime.Release(htmlString);
            if (baseUrlObj != IntPtr.Zero)
                ObjCRuntime.Release(baseUrlObj);
        }
    }

    public void Reload()
    {
        if (_disposed)
            return;

        var reload = ObjCRuntime.GetSelector("reload");
        ObjCRuntime.SendVoid(Handle, reload);
    }

    public void GoBack()
    {
        if (_disposed)
            return;

        var canGoBack = ObjCRuntime.GetSelector("canGoBack");
        if (ObjCRuntime.SendBool(Handle, canGoBack))
        {
            var goBack = ObjCRuntime.GetSelector("goBack");
            ObjCRuntime.SendVoid(Handle, goBack);
        }
    }

    public void GoForward()
    {
        if (_disposed)
            return;

        var canGoForward = ObjCRuntime.GetSelector("canGoForward");
        if (ObjCRuntime.SendBool(Handle, canGoForward))
        {
            var goForward = ObjCRuntime.GetSelector("goForward");
            ObjCRuntime.SendVoid(Handle, goForward);
        }
    }

    public void ExecuteJavaScript(string script, Action<string?>? callback = null)
    {
        if (_disposed || string.IsNullOrEmpty(script))
            return;

        var scriptString = ObjCRuntime.CreateNSString(script);
        if (scriptString == IntPtr.Zero)
            return;

        try
        {
            var evaluateJs = ObjCRuntime.GetSelector("evaluateJavaScript:completionHandler:");

            // For simplicity, passing null for completion handler
            // Full implementation would require creating a block/delegate
            ObjCRuntime.SendVoid(Handle, evaluateJs, scriptString, IntPtr.Zero);
        }
        finally
        {
            ObjCRuntime.Release(scriptString);
        }
    }

    public void SetFrame(double x, double y, double width, double height)
    {
        if (_disposed)
            return;

        var setFrame = ObjCRuntime.GetSelector("setFrame:");
        var frame = new ObjCRuntime.CGRect(x, y, width, height);
        ObjCRuntime.SendVoid(Handle, setFrame, frame);

        // Update layer contents scale for proper Retina rendering
        UpdateContentsScale();
    }

    /// <summary>
    ///     Updates the layer's contents scale to match the window's backing scale factor
    ///     for proper Retina display rendering
    /// </summary>
    public void UpdateContentsScale()
    {
        if (_disposed || Handle == IntPtr.Zero)
            return;

        // Get the window
        var window = ObjCRuntime.SendMessage(Handle, ObjCRuntime.GetSelector("window"));
        if (window == IntPtr.Zero)
            return;

        // Get the backing scale factor (1.0 for normal, 2.0 for Retina)
        var backingScaleFactor = ObjCRuntime.GetDouble(window, ObjCRuntime.GetSelector("backingScaleFactor"));

        // Get the layer and set its contents scale
        var layer = ObjCRuntime.SendMessage(Handle, ObjCRuntime.GetSelector("layer"));
        if (layer != IntPtr.Zero)
            ObjCRuntime.SendDouble(layer, ObjCRuntime.GetSelector("setContentsScale:"), backingScaleFactor);
    }

    /// <summary>
    ///     Sets the page zoom level
    /// </summary>
    public void SetPageZoom(double zoomLevel)
    {
        if (_disposed)
            return;

        ObjCRuntime.SendDouble(Handle, ObjCRuntime.GetSelector("setPageZoom:"), zoomLevel);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        // Release in reverse order of creation: configuration objects first, then the WebView handle
        if (_userContentController != IntPtr.Zero)
        {
            ObjCRuntime.Release(_userContentController);
            _userContentController = IntPtr.Zero;
        }

        if (_preferences != IntPtr.Zero)
        {
            ObjCRuntime.Release(_preferences);
            _preferences = IntPtr.Zero;
        }

        if (_configuration != IntPtr.Zero)
        {
            ObjCRuntime.Release(_configuration);
            _configuration = IntPtr.Zero;
        }

        if (Handle != IntPtr.Zero)
        {
            ObjCRuntime.Release(Handle);
            Handle = IntPtr.Zero;
        }

        if (_webKitHandle != IntPtr.Zero)
        {
            try
            {
                dlclose(_webKitHandle);
            }
            catch
            {
                // Ignore errors during dlclose as Dispose should not throw
            }
            _webKitHandle = IntPtr.Zero;
        }

        GC.SuppressFinalize(this);
    }

    ~WKWebViewWrapper()
    {
        Dispose();
    }
}
