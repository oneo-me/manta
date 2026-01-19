using Avalonia.Controls;
using Avalonia.Platform;
using Manta.Native.Platform;
using Manta.Native.Platform.macOS;

namespace Manta.Native;

public sealed class MantaWebView : NativeControlHost, IDisposable
{
    readonly MantaWebViewImpl _webViewImpl = CreateWebView();

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        if (OperatingSystem.IsMacOS())
            return new PlatformHandle(_webViewImpl.Handle, "NSView");

        return base.CreateNativeControlCore(parent);
    }

    public void LoadUrl(string url)
    {
        _webViewImpl.LoadUrl(url);
    }

    public void LoadHtmlString(string html, string? baseUrl = null)
    {
        _webViewImpl.LoadHtmlString(html, baseUrl);
    }

    static MantaWebViewImpl CreateWebView()
    {
        if (OperatingSystem.IsMacOS())
            return new MacOsWebView();

        throw new PlatformNotSupportedException("MantaWebView is only supported on macOS.");
    }

    public void Dispose()
    {
        _webViewImpl.Dispose();
    }
}
