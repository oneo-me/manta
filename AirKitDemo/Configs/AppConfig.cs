using AirKitDemo.Langs;
using Lumira.Core;

namespace AirKitDemo.Configs;

sealed class AppConfig
{
    public MainWindowConfig MainWindow { get; private set; } = new();
    public LangKey? SelectedSample { get; set; }
    public LangType Lang { get; set; } = Lumira.Lang.Current;
}
