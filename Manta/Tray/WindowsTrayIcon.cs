using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace Manta.Tray;

sealed class WindowsTrayIcon : TrayIconBase
{
    readonly TrayIcon _icon;

    public WindowsTrayIcon(MApp app, byte[] taryIcon)
    {
        _icon = new();
        using var memoryStream = new MemoryStream(taryIcon);
        using var bitmap = new Bitmap(memoryStream);
        _icon.Icon = new(bitmap);
        _icon.Clicked += (_, _) => OnClicked(new(0, 0), new(0, 0)); // TODO Not Implemented get cursor position
        TrayIcon.SetIcons(app, [_icon]);
    }

    public override void Dispose()
    {
    }
}
