using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Concurrency;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using LightKit.Logging;
using LightKit.Logging.Inner;
using LightKit.Reactive.Extensions;
using LightKit.Reactive.Views;

namespace LightKit;

public abstract class LightApp : Application
{
    public static string File { get; }
    public static string Folder { get; }
    public static ReadOnlyCollection<string> CommandLineArgs { get; }

    static LightApp()
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

    public static AppBuilder Build<T>() where T : LightApp, new()
    {
        return AppBuilder.Configure<T>()
            .UsePlatformDetect()
            .With(new FontManagerOptions
            {
                DefaultFamilyName = OperatingSystem.IsMacOS() ? "PingFang SC" : OperatingSystem.IsWindows() ? "Microsoft YaHei UI" : throw new PlatformNotSupportedException()
            })
            .LogToAxe();
    }

    static LightApp? _current;

    public new static LightApp Current => _current ?? throw new("The application is not initialized.");

    internal Action<Exception> OnExceptionAction { get; private set; }

    protected LightApp()
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

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        if (Design.IsDesignMode is false && OperatingSystem.IsMacOS())
        {
            var menu = NativeMenu.GetMenu(this);
            menu?.Items.RemoveAt(0);
        }

        OnInitializeCore(out var mainModel, out var onExit);

        if (Design.IsDesignMode)
            return;

        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            throw new("The application lifetime is not supported.");

        desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
        if (mainModel is not null)
            desktop.MainWindow = mainModel.CreateWindow();

        if (onExit is not null)
            desktop.Exit += delegate
            {
                onExit();
            };
    }

    void OnInitializeCore(out WindowModel? mainModel, out Action? onExit)
    {
        if (Design.IsDesignMode)
            Log.AddLogger(new ConsoleLogger(LogTimeType.None, LogLevel.All));
        else if (Debugger.IsAttached)
            Log.AddLogger(new ConsoleLogger(LogTimeType.Time, LogLevel.All));
        else
            Log.AddLogger(new ConsoleLogger(LogTimeType.DateTime, LogLevel.All));

        OnInitialize(out mainModel, out onExit);
    }

    protected abstract void OnInitialize(out WindowModel? mainModel, out Action? onExit);
}
