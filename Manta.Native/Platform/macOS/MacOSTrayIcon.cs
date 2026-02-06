using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Manta.Native.Platform.macOS;

[SuppressMessage("ReSharper", "InconsistentNaming")]
sealed partial class MacOSTrayIcon : MantaTrayIconImpl
{
    const string LibraryName = "libMantaNative";

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void TrayClickCallback(double x, double y);

    [LibraryImport(LibraryName, EntryPoint = "manta_tray_create")]
    private static unsafe partial long CreateTray(byte* iconData, int iconLength, TrayClickCallback? callback);

    [LibraryImport(LibraryName, EntryPoint = "manta_tray_destroy")]
    private static partial void DestroyTray(long handle);

    TrayClickCallback? _callback;
    readonly long _handle;

    public unsafe MacOSTrayIcon(byte[] trayIcon)
    {
        if (trayIcon == null || trayIcon.Length == 0)
            throw new ArgumentException("Icon data cannot be null or empty.", nameof(trayIcon));

        _callback = OnTrayClicked;

        fixed (byte* iconPtr = trayIcon)
        {
            _handle = CreateTray(iconPtr, trayIcon.Length, _callback);
        }

        if (_handle <= 0)
            throw new InvalidOperationException("Failed to create tray icon - invalid handle");
    }

    void OnTrayClicked(double x, double y)
    {
        OnClicked(new(x - 22, y));
    }

    ~MacOSTrayIcon()
    {
        Dispose();
    }

    public override void Dispose()
    {
        DestroyTray(_handle);
        _callback = null;
        GC.SuppressFinalize(this);
    }
}
