using Avalonia;
using Manta.Native;
using Manta.Reactive.Extensions;
using Manta.Reactive.Views;

namespace Manta;

sealed class MAppTray : IDisposable
{
    readonly MAppTrayOptions _trayOptions;
    TrayWindowModel? _trayWindowModel;
    readonly MantaTrayIcon? _trayIcon;

    public MAppTray(MApp app, MAppTrayOptions trayOptions)
    {
        if (trayOptions.Model is null || trayOptions.Icon is null)
            throw new("TrayModel and Icon must be set when Tray is set.");
        _trayOptions = trayOptions;

        _trayIcon = new(app, trayOptions.Icon);
        _trayIcon.Clicked += TrayIconOnClicked;
    }

    void TrayIconOnClicked(object? sender, Point e)
    {
        if (_trayWindowModel is not null)
        {
            var isVisible = _trayWindowModel.InnerWindow?.IsVisible is true;
            _trayWindowModel.InnerWindow?.Close();
            _trayWindowModel.DisposeIfDisposable();
            _trayWindowModel = null;

            if (isVisible)
                return;
        }

        _trayWindowModel = _trayOptions.Model();
        var trayWindow = _trayWindowModel.CreateWindow();
        trayWindow.Closed += delegate
        {
            trayWindow = null;
        };

        trayWindow.Show();

        var x = (int)e.X;
        var y = (int)e.Y;
        trayWindow.Position = new(x, y);
    }

    public void Dispose()
    {
        if (_trayIcon is null)
            return;

        _trayIcon.Clicked -= TrayIconOnClicked;
        _trayIcon.Dispose();
    }
}
