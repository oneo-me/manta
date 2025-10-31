using LightKit.Controls.Icons;
using LightKit.Reactive.Views;

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
    public virtual IconSource? Icon => null;
    public abstract string Header { get; }
}
