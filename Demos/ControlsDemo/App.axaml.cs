using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ControlsDemo.Langs;
using Axe.I18N;
using Axe.I18N.Avalonia;
using Axe.Reactive;
using ControlsDemo.Services;
using ControlsDemo.Views;

namespace ControlsDemo;

sealed class App : OrphicApp
{
    public App()
    {
        AccentColor = Colors.Teal;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnInitialize(out WindowModel? mainModel, out Action? onExit)
    {
        Lang.Register(LangData.LoadFromAssembly<LangKey>(typeof(App).Assembly));

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
