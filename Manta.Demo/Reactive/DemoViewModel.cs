using Manta.Controls.Icons;
using Manta.Reactive.Views;

namespace Manta.Demo.Reactive;

enum DemoGroup
{
    Styles,
    Controls,
    Reactive,
    MultiLanguage
}

enum DemoSubGroup
{
    None,
    Chart,
    Window,
    Button,
    Text,
    Progress,
    List,
    Switch,
    Icon,
    Base,
    Other
}

abstract class DemoViewModel : ViewModel
{
    public abstract DemoGroup Group { get; }
    public abstract DemoSubGroup SubGroup { get; }
    public virtual IconSource? Icon => null;
    public abstract string Header { get; }
}
