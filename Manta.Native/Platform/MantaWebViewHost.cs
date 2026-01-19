using Avalonia.Controls;
using Avalonia.Platform;
using Manta.Native.Platform.macOS;

namespace Manta.Native.Platform;

public sealed class MantaWebViewHost : NativeControlHost, IDisposable
{
    readonly MantaWebView _webView = CreateWebView();

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        if (OperatingSystem.IsMacOS())
            return new PlatformHandle(_webView.Handle, "NSView");

        return base.CreateNativeControlCore(parent);
    }

    public void LoadUrl(string url)
    {
        _webView.LoadUrl(url);
    }

    public void LoadHtmlString(string html, string? baseUrl = null)
    {
        _webView.LoadHtmlString(html, baseUrl);
    }

    static MantaWebView CreateWebView()
    {
        if (OperatingSystem.IsMacOS())
            return new MacOSWebView();

        throw new PlatformNotSupportedException("MantaWebView is only supported on macOS.");
    }

    public void Dispose()
    {
        _webView.Dispose();
    }
}
