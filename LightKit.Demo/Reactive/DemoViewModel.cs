using LightKit.Controls.Icons;
using LightKit.Reactive.Views;
using LightKit.Demo.Langs;

namespace LightKit.Demo.Reactive;

enum DemoGroup
{
    Styles,
    Controls,
    Reactive,
    MultiLanguage
}

abstract class DemoViewModel : ViewModel
{
    public abstract DemoGroup Group { get; }
    public abstract IconSource? Icon { get; }
    public abstract LangKey Header { get; }
}
