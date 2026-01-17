using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Platform;

namespace Manta.WebView.Platform.macOS;

/// <summary>
///     NativeControlHost subclass that embeds WKWebView on macOS
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
sealed class MacOSNativeControlHost : NativeControlHost
{
    public WKWebViewWrapper? WebView { get; private set; }

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        if (OperatingSystem.IsMacOS())
        {
            WebView = new();
            var handle = WebView.Handle;

            // Validate the handle is a proper NSView
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create WKWebView - handle is null");

            // Check if it's actually an NSView
            if (!ObjCRuntime.IsKindOfClass(handle, "NSView"))
                throw new InvalidOperationException("WKWebView handle is not an NSView");

            return new PlatformHandle(handle, "NSView");
        }

        return base.CreateNativeControlCore(parent);
    }

    protected override void DestroyNativeControlCore(IPlatformHandle control)
    {
        WebView?.Dispose();
        WebView = null;
        base.DestroyNativeControlCore(control);
    }
}
