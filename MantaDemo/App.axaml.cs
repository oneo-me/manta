using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using Avalonia.Markup.Xaml;
using Manta;
using Manta.Globalization;
using Manta.Logging;
using Manta.Reactive.Extensions;
using Manta.Reactive.Services;
using Manta.Reactive.Views;
using Manta.Services;
using MantaDemo.Services;
using MantaDemo.Views;

namespace MantaDemo;

sealed class App : MApp
{
    public static Assets Assets { get; } = new(typeof(App).Assembly);

    public App()
    {
        Name = nameof(Manta);
#if DEBUG
        SupportActivatableLifetime = Debugger.IsAttached is false;
#else
        SupportActivatableLifetime = true;
#endif
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnException(Exception exception)
    {
        Log.Error("An unhandled exception occurred: {Exception}", exception);
        base.OnException(exception);
    }

    [SuppressMessage("Reliability", "CA2000:丢失范围之前释放对象")]
    protected override void OnInitialize(CompositeDisposable disposables, out Func<WindowModel>? mainModel)
    {
        var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var dataFolder = Path.Combine(userFolder, ".oneo", "MantaDemo");
        var cacheFolder = Path.Combine(dataFolder, "Cache");
        Directory.CreateDirectory(dataFolder);
        Directory.CreateDirectory(cacheFolder);

        var fileLogger = new FileLogger(Path.Combine(dataFolder, "logs.log"));
        Log.AddLogger(fileLogger);

        Log.Information("MantaDemo started.");

        Lang.Register(LangData.LoadFromAssembly(typeof(App).Assembly));

        var imageLoadService = Service.GetService<ImageLoadService>();
        imageLoadService.CacheDirectory = cacheFolder;

        var configService = Service.Register(new ConfigService(Path.Combine(dataFolder, "config.json")));
        configService.DisposeWith(disposables);

        Service.Register(new DemoService());

        // Register ViewService at last, because it depends on ConfigService and DemoService.
        var viewService = Service.Register(new ViewService(configService));
        viewService.DisposeWith(disposables);

        Lang.Current = configService.Config.Language;

        Disposable.Create(() => Log.Information("MantaDemo stopped.")).DisposeWith(disposables);
        fileLogger.DisposeWith(disposables);

        mainModel = () => new Main_Model();
    }
}
