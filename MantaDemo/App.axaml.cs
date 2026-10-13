using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Manta;
using Manta.Globalization;
using Manta.Logging;
using Manta.Reactive.Services;
using Manta.Services;
using MantaDemo.Services;
using MantaDemo.Views;
using LangData = MantaDemo.Langs.LangData;

namespace MantaDemo;

sealed class App : MApp
{
    public static Assets Assets { get; } = new(typeof(App).Assembly);

    public App()
    {
        Name = nameof(MantaDemo);
        Icon = Assets.GetBitmap("AppIcon.png");
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

    protected override void OnInitialize(out MAppOptions options)
    {
        Lang.Register(LangData.Load());

        var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var dataFolder = Path.Combine(userFolder, ".oneo", Name);
        Directory.CreateDirectory(dataFolder);

        var cacheFolder = Path.Combine(dataFolder, "Cache");
        Directory.CreateDirectory(cacheFolder);

        FileLogger? fileLogger = null;

        if (Design.IsDesignMode is false)
        {
            fileLogger = new(Path.Combine(dataFolder, "logs.log"));
            Log.AddLogger(fileLogger);
        }

        Log.Information("MantaDemo started.");

        var imageLoadService = Service.GetService<ImageLoadService>();
        imageLoadService.CacheDirectory = cacheFolder;

        var configService = Service.Register(new ConfigService(Path.Combine(dataFolder, "config.json")));
        Service.Register(new DemoService());

        var viewService = Service.Register(new ViewService(configService));

        Lang.Current = configService.Config.Language;

        options = new()
        {
            MainModel = () => new Main_Model(),
            Exit = () =>
            {
                Log.Information("MantaDemo stopped.");

                viewService.Dispose();
                configService.Dispose();
                fileLogger?.Dispose();
            }
        };
    }
}
