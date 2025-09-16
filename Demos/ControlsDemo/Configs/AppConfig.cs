using ControlsDemo.Langs;
using Axe.I18N.Core;

namespace ControlsDemo.Configs;

sealed class AppConfig
{
    public MainWindowConfig MainWindow { get; private set; } = new();
    public LangKey? SelectedSample { get; set; }
    public LangType Lang { get; set; } = Axe.I18N.Lang.Current;
}
