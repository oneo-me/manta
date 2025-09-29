using System.Reactive.Concurrency;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Axe.Reactive.Extensions;
using Axe.Reactive.Views;

namespace Axe.Reactive;

public abstract class AxeApp : Application
{
    public new static AxeApp Current => Application.Current as AxeApp ?? throw new InvalidOperationException($"Application is not {nameof(AxeApp)}");

    internal Action<Exception> OnExceptionAction { get; private set; }

    protected AxeApp()
    {
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
        if (Design.IsDesignMode)
            return;

        AvaloniaScheduler.Instance.Schedule(() => throw new("An unhandled exception occurred", exception));
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

        OnInitialize(out var mainModel, out var onExit);

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

    protected abstract void OnInitialize(out WindowModel? mainModel, out Action? onExit);
}
