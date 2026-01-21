using Avalonia.Controls;
using Manta.Controls;
using Manta.Controls.Icons;
using Manta.Demo.Langs;
using Manta.Demo.Services;
using Manta.Globalization;
using Manta.Reactive.Services;
using Manta.Reactive.Views;
using Manta.Tools;

namespace Manta.Demo.Views;

[View<TweakcnTool>]
public class TweakcnTool_Model : WindowModel
{
    public static TweakcnTool_Model Design => new();

    readonly ConfigService _configService = Service.GetService<ConfigService>();

    public string Input
    {
        get;
        set => SetValue(ref field, value);
    } = string.Empty;

    public IconSource ExportIcon { get; } = IconSource.FromAsset(Assets.GetUri("Icons/Upload--Streamline-Solar-Ar.svg"));

    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);

        window[!Window.TitleProperty] = Lang.GetBinding(new(LangKey.TweakcnTool));
        window.MinWidth = window.Width = 960;
        window.MinHeight = window.Height = 720;

        Input = _configService.Config.Tools.Tweakcn.Input;

        window.Closed += delegate
        {
            _configService.Config.Tools.Tweakcn.Input = Input;
        };
    }

    public async Task ExportAsync()
    {
        var storageProvider = await this.GetStorageProviderAsync();
        var storageFolders = await storageProvider.OpenFolderPickerAsync(new()
        {
            AllowMultiple = false
        });
        if (storageFolders.Count <= 0)
            return;

        var folder = storageFolders[0];
        var lightFile = await folder.CreateFileAsync("@Light.axaml");
        var darkFile = await folder.CreateFileAsync("@Dark.axaml");
        if (lightFile is null || darkFile is null)
            return;

        var result = TweakcnParser.Parser(Input.Split('\n'));
        var lightStr = result.LightTheme.ToXaml(string.Empty, true);
        var darkStr = result.DarkTheme.ToXaml(lightStr, true);

        var lightStream = await lightFile.OpenWriteAsync();
        await using (var writer = new StreamWriter(lightStream))
        {
            await writer.WriteAsync(lightStr);
        }

        var darkStream = await darkFile.OpenWriteAsync();
        await using (var writer = new StreamWriter(darkStream))
        {
            await writer.WriteAsync(darkStr);
        }
    }
}
