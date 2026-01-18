namespace Manta.Native.Platform.macOS;

/// <summary>
///     Wrapper for macOS NSStatusItem using the MantaNative Swift library.
/// </summary>
sealed class NativeTrayWrapper : IDisposable
{
    readonly long _handle;
    bool _disposed;

    // Keep a reference to the callback to prevent GC collection
    MantaNative.TrayClickCallback? _callback;

    /// <summary>
    ///     Event raised when the tray icon is clicked.
    /// </summary>
    public event EventHandler<TrayClickEventArgs>? Clicked;

    /// <summary>
    ///     Creates a new native tray icon.
    /// </summary>
    /// <param name="icon">PNG data for icon.</param>
    /// <param name="isTemplate">Whether to treat the icon as a template image (true for monochrome icons that should adapt to system theme).</param>
    public unsafe NativeTrayWrapper(byte[] icon, bool isTemplate = true)
    {
        if (icon == null || icon.Length == 0)
            throw new ArgumentException("Icon data cannot be null or empty.", nameof(icon));

        _callback = OnTrayClicked;

        fixed (byte* iconPtr = icon)
        {
            _handle = MantaNative.CreateTray(
                iconPtr,
                icon.Length,
                isTemplate,
                _callback);
        }

        if (_handle <= 0)
            throw new InvalidOperationException("Failed to create tray icon - invalid handle");
    }

    void OnTrayClicked(double x, double y, double screenX, double screenY)
    {
        Clicked?.Invoke(this, new(new(x, y), new(screenX, screenY)));
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        MantaNative.DestroyTray(_handle);
        _callback = null;

        GC.SuppressFinalize(this);
    }

    ~NativeTrayWrapper()
    {
        Dispose();
    }
}
