using AirKit.I18N.Enums;
using AirKitDemo.Langs;

namespace AirKitDemo.Configs;

sealed class AppConfig
{
    public MainWindowConfig MainWindow { get; private set; } = new();
    public LangKey? SelectedSample { get; set; }
    public LangType Lang { get; set; } = AirKit.I18N.Lang.Current;
}
