using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;
using AirKit.Extensions;
using AirKit.Logging;
using AirKit.Themes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace AirKit;

public abstract class AppBase : Application
{
    internal Action<Exception> OnExceptionAction { get; private set; }

    protected AppBase()
    {
        base.Name = GetType().Assembly.GetName().Name;

        OnExceptionAction = OnException;

        Dispatcher.UIThread.UnhandledException += (_, e) =>
        {
            e.Handled = true;
            OnExceptionCore(e.Exception, false);
        };
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is Exception exception)
                OnExceptionCore(exception, true);
        };
    }

    static void OnExceptionCore(Exception exception, bool isFatal)
    {
        Log.WriteLine(isFatal ? LogLevel.Fatal : LogLevel.Error, "App", "An unhandled exception occurred. \n{Exception}", exception);

        if (Design.IsDesignMode)
            return;

        if (Debugger.IsAttached)
            AvaloniaScheduler.Instance.Schedule(() => throw new("An unhandled exception occurred", exception));
    }

    protected virtual void OnException(Exception exception)
    {
        OnExceptionCore(exception, false);
    }

    public Bitmap? Icon { get; protected init; }

    public new string Name
    {
        get => base.Name ?? string.Empty;
        protected init => base.Name = value;
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public Color? AccentColor
    {
        get => SystemAccentColors.AppSystemAccentColor;
        init => SystemAccentColors.AppSystemAccentColor = value;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        if (Design.IsDesignMode is false && OperatingSystem.IsMacOS())
        {
            var menu = NativeMenu.GetMenu(this);
            menu?.Items.RemoveAt(0);
        }

        OnInitialize(out var mainModel, out var onExit);

        if (Design.IsDesignMode)
        {
            Log.Information("App", "Design mode is enabled");
            return;
        }

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

    protected abstract void OnInitialize(out WindowModel? mainModel, out Action? onExit);
}
