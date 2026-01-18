using Manta.Controls;

namespace Manta.Reactive.Views;

public abstract class WindowModel : ViewModel
{
    internal MWindow? InnerWindow { get; set; }

    public virtual void OnWindowInitialized(MWindow window)
    {
    }
}
