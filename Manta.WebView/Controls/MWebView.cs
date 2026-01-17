using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Manta.Logging;
using Manta.WebView.Platform.macOS;

namespace Manta.WebView.Controls;

public class MWebView : TemplatedControl
{
    MacOSNativeControlHost? _nativeHost;
    bool _isAttached;

    public static readonly StyledProperty<string?> UrlProperty =
        AvaloniaProperty.Register<MWebView, string?>(nameof(Url));

    public static readonly StyledProperty<string?> HtmlContentProperty =
        AvaloniaProperty.Register<MWebView, string?>(nameof(HtmlContent));

    public string? Url
    {
        get => GetValue(UrlProperty);
        set => SetValue(UrlProperty, value);
    }

    public string? HtmlContent
    {
        get => GetValue(HtmlContentProperty);
        set => SetValue(HtmlContentProperty, value);
    }

    static MWebView()
    {
        UrlProperty.Changed.AddClassHandler<MWebView>((x, e) => x.OnUrlChanged(e));
        HtmlContentProperty.Changed.AddClassHandler<MWebView>((x, e) => x.OnHtmlContentChanged(e));
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _nativeHost = e.NameScope.Find<MacOSNativeControlHost>("PART_NativeHost");
        Log.Debug($"OnApplyTemplate: _nativeHost={_nativeHost}");
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        // Defer content loading to ensure native control is fully initialized
        Dispatcher.UIThread.Post(() =>
        {
            _isAttached = true;
            LoadInitialContent();
        }, DispatcherPriority.Loaded);
    }

    void LoadInitialContent()
    {
        var webView = _nativeHost?.WebView;
        Log.Debug($"LoadInitialContent: WebView={webView}, Url={Url}, HtmlContent={HtmlContent}");

        if (webView == null)
        {
            Log.Debug("WebView is null, cannot load content");
            return;
        }

        if (!string.IsNullOrEmpty(Url))
        {
            Log.Debug($"Loading URL: {Url}");
            webView.LoadUrl(Url);
        }
        else if (!string.IsNullOrEmpty(HtmlContent))
        {
            Log.Debug("Loading HTML content");
            webView.LoadHtmlString(HtmlContent);
        }
    }

    void OnUrlChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var url = e.NewValue as string;
        Log.Debug($"OnUrlChanged: {url}, _isAttached={_isAttached}, WebView={_nativeHost?.WebView}");

        // Only load URL if already attached and WebView is ready
        if (_isAttached && !string.IsNullOrEmpty(url) && _nativeHost?.WebView != null)
            _nativeHost.WebView.LoadUrl(url);
    }

    void OnHtmlContentChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var html = e.NewValue as string;
        if (_isAttached && !string.IsNullOrEmpty(html) && string.IsNullOrEmpty(Url) && _nativeHost?.WebView != null)
            _nativeHost.WebView.LoadHtmlString(html);
    }

    public void Reload()
    {
        _nativeHost?.WebView?.Reload();
    }

    public void GoBack()
    {
        _nativeHost?.WebView?.GoBack();
    }

    public void GoForward()
    {
        _nativeHost?.WebView?.GoForward();
    }

    public void ExecuteJavaScript(string script, Action<string?>? callback = null)
    {
        _nativeHost?.WebView?.ExecuteJavaScript(script, callback);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _isAttached = false;
    }
}
