using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Manta.Reactive.Views;

namespace Manta;

sealed class MAppLifetime
{
    readonly MApp _app;
    readonly IClassicDesktopStyleApplicationLifetime _desktop;
    readonly bool _isActivatableLifetime;
    public event EventHandler? Exit;

    public MAppLifetime(MApp app, Func<WindowModel>? func)
    {
        if (app.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            throw new("The application lifetime is not supported.");

        _app = app;
        _desktop = desktop;

        if (app.SupportActivatableLifetime && app.TryGetFeature(typeof(IActivatableLifetime)) is IActivatableLifetime activatableLifetime)
        {
            _isActivatableLifetime = true;
            activatableLifetime.Activated += ActivatableLifetimeOnActivated;
        }

        _desktop.ShutdownMode = _isActivatableLifetime ? ShutdownMode.OnExplicitShutdown : ShutdownMode.OnMainWindowClose;

        if (func is not null)
        {
            var mainWindow = func().CreateWindow();
            mainWindow.IsMainWindow = true;
            _desktop.MainWindow = mainWindow;

            if (_isActivatableLifetime)
                _desktop.MainWindow.Closing += MainWindowOnClosing;
        }

        _desktop.Exit += DesktopOnExit;
    }

    void MainWindowOnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (e.CloseReason == WindowCloseReason.ApplicationShutdown)
            return;

        e.Cancel = true;
        _desktop.MainWindow?.Hide();
    }

    void ActivatableLifetimeOnActivated(object? sender, ActivatedEventArgs e)
    {
        if (e.Kind != ActivationKind.Reopen)
            return;

        _app.Activate();
    }

    void DesktopOnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        Exit?.Invoke(this, EventArgs.Empty);
    }
}
