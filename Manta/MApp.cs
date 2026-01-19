using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Concurrency;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Manta.Controls;
using Manta.Extensions;
using Manta.Globalization;
using Manta.Langs;
using Manta.Logging;
using Manta.Logging.Inner;
using Manta.Native;
using Manta.Reactive.Extensions;
using Manta.Reactive.Services;
using Manta.Reactive.Views;
using Manta.Services;

namespace Manta;

public abstract class MApp : Application
{
    public static string File { get; }
    public static string Folder { get; }
    public static ReadOnlyCollection<string> CommandLineArgs { get; }

    static MApp()
    {
        var args = Environment.GetCommandLineArgs();
        File = args[0];

        if (string.Equals(Path.GetFileNameWithoutExtension(File), "Avalonia.Designer.HostApp", StringComparison.Ordinal))
        {
            CommandLineArgs = new([]);
            File = args[^1];
        }
        else
        {
            CommandLineArgs = new(args[1..]);
        }

        Folder = Path.GetDirectoryName(File)!;
    }

    public static AppBuilder Build<T>() where T : MApp, new()
    {
        return AppBuilder.Configure<T>()
            .UsePlatformDetect()
            .With(new FontManagerOptions
            {
                DefaultFamilyName = OperatingSystem.IsMacOS() ? "PingFang SC" : OperatingSystem.IsWindows() ? "Microsoft YaHei UI" : throw new PlatformNotSupportedException()
            })
            .LogToAxe();
    }

    static MApp? _current;

    public new static MApp Current => _current ?? throw new("The application is not initialized.");

    internal Action<Exception> OnExceptionAction { get; private set; }

    protected MApp()
    {
        _current = this;

        base.Name = GetType().Assembly.GetName().Name;

        OnExceptionAction = OnException;

        Dispatcher.UIThread.UnhandledException += (_, e) =>
        {
            e.Handled = true;
            OnException(e.Exception);
        };
        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            e.SetObserved();
            OnException(e.Exception);
        };
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is Exception exception)
                OnException(exception);
        };
    }

    protected virtual void OnException(Exception exception)
    {
        Log.Error("An unhandled exception occurred: {0}", exception);

        if (Design.IsDesignMode)
            return;

        AvaloniaScheduler.Instance.Schedule(() => throw new("An unhandled exception has occurred.", exception));
    }

    public Bitmap? Icon { get; protected init; }

    public new string Name
    {
        get => base.Name ?? string.Empty;
        protected init => base.Name = value;
    }

    protected bool SupportActivatableLifetime { get; init; } = false;

    public void Activate()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        if (desktop.MainWindow is null)
            return;

        if (desktop.MainWindow.IsVisible)
            desktop.MainWindow.FocusActivate();
        else
            desktop.MainWindow.Show();
    }

    public void Exit()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        desktop.Shutdown();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        Lang.Register(LangData.LoadFromAssembly(typeof(MApp).Assembly));

        if (Design.IsDesignMode is false && OperatingSystem.IsMacOS())
        {
            var menu = NativeMenu.GetMenu(this);
            if (menu is null)
                return;

            menu.Items.RemoveAt(0);

            foreach (var item in menu.Items.OfType<NativeMenuItem>())
                if (item.Header?.StartsWith("Services") is true)
                    item[!NativeMenuItem.HeaderProperty] = Lang.GetBinding(new(InnerLangKey.AppMenu_Services));
                else if (item.Header?.StartsWith("Hide") is true)
                    item[!NativeMenuItem.HeaderProperty] = Lang.GetBinding(new(InnerLangKey.AppMenu_Hide_App, Name));
                else if (item.Header?.StartsWith("Hide Other") is true)
                    item[!NativeMenuItem.HeaderProperty] = Lang.GetBinding(new(InnerLangKey.AppMenu_Hide_Others));
                else if (item.Header?.StartsWith("Show All") is true)
                    item[!NativeMenuItem.HeaderProperty] = Lang.GetBinding(new(InnerLangKey.AppMenu_Show_All));
                else if (item.Header?.StartsWith("Quit") is true)
                    item[!NativeMenuItem.HeaderProperty] = Lang.GetBinding(new(InnerLangKey.AppMenu_Quit, Name));
        }

        OnInitializeInternal();
        OnInitializeCore(out var initData);

        if (Design.IsDesignMode)
            return;

        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            throw new("The application lifetime is not supported.");

        var isActivatableLifetime = false;
        if (SupportActivatableLifetime && TryGetFeature(typeof(IActivatableLifetime)) is IActivatableLifetime activatableLifetime)
        {
            isActivatableLifetime = true;
            activatableLifetime.Activated += (_, e) =>
            {
                if (e.Kind != ActivationKind.Reopen)
                    return;

                Activate();
            };
        }

        desktop.ShutdownMode = isActivatableLifetime ? ShutdownMode.OnExplicitShutdown : ShutdownMode.OnMainWindowClose;

        if (initData.MainModel is not null)
        {
            var mainWindow = initData.MainModel().CreateWindow();
            mainWindow.Variant = MWindowVariant.MainWindow;
            desktop.MainWindow = mainWindow;

            if (isActivatableLifetime)
                desktop.MainWindow.Closing += (_, args) =>
                {
                    if (args.CloseReason == WindowCloseReason.ApplicationShutdown)
                        return;

                    args.Cancel = true;
                    desktop.MainWindow.Hide();
                };
        }

        if (initData.Tary is not null)
        {
            if (initData.Tary.Model is null || initData.Tary.Icon is null)
                throw new("TrayModel and Icon must be set when Tary is set.");

            _trayIcon = new(this, initData.Tary.Icon);
            _trayIcon.Clicked += (_, e) =>
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

                _trayWindowModel = initData.Tary.Model();
                var trayWindow = _trayWindowModel.CreateWindow();
                trayWindow.Closed += delegate
                {
                    trayWindow = null;
                };

                trayWindow.Show();

                var x = (int)e.X;
                var y = (int)e.Y;
                trayWindow.Position = new(x, y);
            };
        }

        if (initData.Exit is not null)
            desktop.Exit += delegate
            {
                _trayIcon?.Dispose();
                initData.Exit();
            };
    }

    TrayWindowModel? _trayWindowModel;
    MantaTrayIcon? _trayIcon;

    static void OnInitializeInternal()
    {
        Service.Register(new ImageLoadService());
    }

    void OnInitializeCore(out MAppOptions options)
    {
        if (Design.IsDesignMode)
            Log.AddLogger(new ConsoleLogger(LogTimeType.None, LogLevel.All));
        else if (Debugger.IsAttached)
            Log.AddLogger(new ConsoleLogger(LogTimeType.Time, LogLevel.All));
        else
            Log.AddLogger(new ConsoleLogger(LogTimeType.DateTime, LogLevel.All));

        Lang.Register(LangData.LoadFromAssembly(typeof(MApp).Assembly));

        OnInitialize(out options);
    }

    protected abstract void OnInitialize(out MAppOptions options);
}
