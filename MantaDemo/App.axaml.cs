using System.Diagnostics;
using Avalonia.Markup.Xaml;
using Manta;
using Manta.Globalization;
using Manta.Reactive.Services;
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

    protected override void OnInitialize(out MAppOptions options)
    {
        Lang.Register(LangData.LoadFromAssembly(typeof(App).Assembly));

        var appFolder = Folder;
        var dataFolder = Path.Combine(appFolder, "Data");
        var cacheFolder = Path.Combine(dataFolder, "Cache");
        Directory.CreateDirectory(dataFolder);
        Directory.CreateDirectory(cacheFolder);

        var imageLoadService = Service.GetService<ImageLoadService>();
        imageLoadService.CacheDirectory = cacheFolder;

        var configService = Service.Register(new ConfigService(Path.Combine(dataFolder, "config.yml")));
        Service.Register(new DemoService());

        // Register ViewService at last, because it depends on ConfigService and DemoService.
        var viewService = Service.Register(new ViewService(configService));

        Lang.Current = configService.Config.Language;

        options = new()
        {
            MainModel = () => new Main_Model(),
            Exit = () =>
            {
                viewService.Save();
                configService.Save();
            }
        };
    }
}
