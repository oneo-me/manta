using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace Manta.Native.Platform.Windows;

sealed class WindowsTrayIcon : MantaTrayIconImpl
{
    readonly TrayIcon _icon;

    public WindowsTrayIcon(Application app, byte[] trayIcon)
    {
        _icon = new();
        using var memoryStream = new MemoryStream(trayIcon);
        using var bitmap = new Bitmap(memoryStream);
        _icon.Icon = new(bitmap);
        _icon.Clicked += (_, _) => OnClicked(new(0, 0)); // TODO Not Implemented get cursor position
        TrayIcon.SetIcons(app, [_icon]);
    }

    public override void Dispose()
    {
        _icon.Dispose();
    }
}
