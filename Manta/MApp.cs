using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Security.Cryptography;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Manta.Extensions;
using Manta.Globalization;
using Manta.Logging;
using Manta.Reactive.Extensions;
using Manta.Reactive.Services;
using Manta.Services;

namespace Manta;

public abstract partial class MApp : Application
{
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

    protected internal bool SupportActivatableLifetime { get; init; }
    protected internal bool SupportMultipleInstances { get; init; }

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

    MAppLifetime? _lifetime;
    MAppSingleInstance? _singleInstance;

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        if (Design.IsDesignMode is false && SupportMultipleInstances is false)
        {
            var instanceName = GetType().FullName!;
            var hash = MD5.HashData(Encoding.UTF8.GetBytes(instanceName));
            instanceName = Convert.ToHexString(hash);

            _singleInstance = new($"{nameof(Manta)}_{instanceName}");
            _singleInstance.ReOpen += (_, args) => OnReOpen(args);
            if (!_singleInstance.IsNew)
                return;
        }

        MAppMenus.RegisterDefaultMenus(this);

        OnInitializeInternal();
        OnInitializeCore(out var initData);

        if (Design.IsDesignMode)
            return;

        _lifetime = new(this, initData.MainModel);

        _lifetime.Exit += delegate
        {
            _singleInstance?.Dispose();
            initData.Exit?.Invoke();
        };
    }

    protected virtual void OnReOpen(string[] args)
    {
        _lifetime?.Activate();
    }

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
