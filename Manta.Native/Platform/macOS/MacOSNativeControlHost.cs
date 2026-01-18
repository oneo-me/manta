using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Platform;

namespace Manta.Native.Platform.macOS;

/// <summary>
///     NativeControlHost subclass that embeds WKWebView on macOS using the MantaNative library.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
sealed class MacOSNativeControlHost : NativeControlHost
{
    public NativeWebViewWrapper WebView { get; } = new();

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        if (OperatingSystem.IsMacOS())
        {
            var handle = WebView.Handle;

            // Validate the handle is valid
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create WKWebView - handle is null");

            return new PlatformHandle(handle, "NSView");
        }

        return base.CreateNativeControlCore(parent);
    }
}
