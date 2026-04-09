using Manta.Controls.Icons;
using Manta.Reactive.Views;

namespace MantaDemo.Reactive;

enum DemoGroup
{
    Styles,
    Controls,
    Reactive,
    MultiLanguage
}

enum DemoSubGroup
{
    Window,
    Custom,
    Advanced,
    Base,
    None
}

abstract class DemoViewModel : ViewModel
{
    public abstract DemoGroup Group { get; }
    public abstract DemoSubGroup SubGroup { get; }
    public virtual IconSource? Icon => null;
    public abstract string Header { get; }
}
