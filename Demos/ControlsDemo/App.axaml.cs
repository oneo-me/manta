using Avalonia.Markup.Xaml;
using Axe.I18N;
using Axe.I18N.Avalonia;
using Axe.Reactive;
using ControlsDemo.Langs;
using ControlsDemo.Services;
using ControlsDemo.Views;

namespace ControlsDemo;

sealed class App : AxeApp
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnInitialize(out WindowModel? mainModel, out Action? onExit)
    {
        Lang.Register(LangData.LoadFromAssembly<LangKey>());

        var configFile = Path.Combine(DesktopApp.Folder, "config.yaml");
        var configService = Service.Register(new ConfigService(configFile));

        Lang.Current = configService.Config.Lang;

        mainModel = new Main_Model();
        onExit = () =>
        {
            configService.Save();
        };
    }
}
