using Axe.Icons;
using Axe.Reactive.Views;
using AxeDemo.Langs;

namespace AxeDemo.Reactive;

enum DemoGroup
{
    Styles,
    Controls,
    Reactive,
    MultiLanguage
}

abstract class DemoViewModel : ViewModel
{
    public abstract IconSource Icon { get; }
    public abstract LangKey Header { get; }
    public abstract DemoGroup Group { get; }
}
