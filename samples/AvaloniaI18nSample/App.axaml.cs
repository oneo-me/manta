using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Manta.Globalization;
using LangData = AvaloniaI18nSample.Langs.LangData;

namespace AvaloniaI18nSample;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // The only Manta call. It registers the translations generated from Langs/*.yml
        // and picks the language from the system culture.
        Lang.Register(LangData.Load());

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();

        base.OnFrameworkInitializationCompleted();
    }
}
