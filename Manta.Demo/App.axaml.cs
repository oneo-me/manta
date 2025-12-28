using Avalonia.Markup.Xaml;
using Manta.Globalization;
using Manta.Reactive.Services;
using Manta.Reactive.Views;
using Manta.Demo.Services;
using Manta.Demo.Views;

namespace Manta.Demo;

sealed class App : LightApp
{
    public App()
    {
        Name = nameof(Manta);
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnInitialize(out WindowModel? mainModel, out Action? onExit)
    {
        var appFolder = Folder;
        var dataFolder = Path.Combine(appFolder, "Data");
        Directory.CreateDirectory(dataFolder);

        Lang.Register(LangData.LoadFromAssembly(typeof(App).Assembly));

        var configService = Service.Register(new ConfigService(Path.Combine(dataFolder, "config.json")));
        Service.Register(new DemoService());

        // Register ViewService at last, because it depends on ConfigService and DemoService.
        var viewService = Service.Register(new ViewService(configService));

        Lang.Current = configService.Config.Language;

        mainModel = new Main_Model();
        onExit = () =>
        {
            viewService.Save();
            configService.Save();
        };
    }
}
