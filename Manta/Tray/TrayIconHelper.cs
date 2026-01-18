namespace Manta.Tray;

static class TrayIconHelper
{
    public static TrayIconBase CreateTrayIcon(MApp app, byte[] icon)
    {
        if (OperatingSystem.IsWindows())
            return new WindowsTrayIcon(app, icon);

        if (OperatingSystem.IsMacOS())
            return new MacOSTrayIcon(icon);

        throw new PlatformNotSupportedException("Tray icon is not supported on this platform.");
    }
}
