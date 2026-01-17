using System.Diagnostics;
using Avalonia.Markup.Xaml;
using Manta.Demo.Services;
using Manta.Demo.Views;
using Manta.Globalization;
using Manta.Reactive.Services;
using Manta.Services;

namespace Manta.Demo;

sealed class App : MApp
{
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
        var appFolder = Folder;
        var dataFolder = Path.Combine(appFolder, "Data");
        var cacheFolder = Path.Combine(dataFolder, "Cache");
        Directory.CreateDirectory(dataFolder);
        Directory.CreateDirectory(cacheFolder);

        var imageLoadService = Service.GetService<ImageLoadService>();
        imageLoadService.CacheDirectory = cacheFolder;

        Lang.Register(LangData.LoadFromAssembly(typeof(App).Assembly));

        var configService = Service.Register(new ConfigService(Path.Combine(dataFolder, "config.json")));
        Service.Register(new DemoService());

        // Register ViewService at last, because it depends on ConfigService and DemoService.
        var viewService = Service.Register(new ViewService(configService));

        Lang.Current = configService.Config.Language;

        options = new()
        {
            MainModel = new Main_Model(),
            Tary = new()
            {
                Model = new TrayView_Model(),
                Icon = Assets.GetBytes("TrayIcon.png")
            },
            Exit = () =>
            {
                viewService.Save();
                configService.Save();
            }
        };
    }
}
