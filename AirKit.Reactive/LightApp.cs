using System.Diagnostics;
using AirKit.Reactive.Logging;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace AirKit.Reactive;

public abstract class LightApp : Application
{
    public new static LightApp Current { get; } = Application.Current as LightApp ?? throw new InvalidOperationException("Application is not LightApp");

    internal Action<Exception> OnExceptionAction { get; private set; }

    protected LightApp()
    {
        OnExceptionAction = OnException;

        ObservableSystem.RegisterUnhandledExceptionHandler(OnException);
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
            Dispatcher.UIThread.Invoke(() => throw exception);
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

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        if (Design.IsDesignMode is false && OperatingSystem.IsMacOS())
        {
            var menu = NativeMenu.GetMenu(this);
            menu?.Items.RemoveAt(0);
        }
    }
}
