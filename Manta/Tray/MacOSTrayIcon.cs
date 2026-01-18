using System.Diagnostics.CodeAnalysis;
using Manta.Native.Platform.macOS;

namespace Manta.Tray;

[SuppressMessage("ReSharper", "InconsistentNaming")]
sealed class MacOSTrayIcon : TrayIconBase, IDisposable
{
    readonly NativeTrayWrapper _nativeTray;
    bool _disposed;

    public MacOSTrayIcon(byte[] icon)
    {
        // isTemplate: true - macOS will automatically adjust icon color based on menu bar appearance
        _nativeTray = new(icon);
        _nativeTray.Clicked += OnNativeTrayClicked;
    }

    void OnNativeTrayClicked(object? sender, TrayClickEventArgs e)
    {
        OnClicked(e.Position, e.ScreenOrigin);
    }

    public override void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        _nativeTray.Clicked -= OnNativeTrayClicked;
        _nativeTray.Dispose();

        GC.SuppressFinalize(this);
    }

    ~MacOSTrayIcon()
    {
        Dispose();
    }
}
