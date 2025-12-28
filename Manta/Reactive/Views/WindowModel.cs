using Avalonia.Controls;
using Manta.Controls;

namespace Manta.Reactive.Views;

public abstract class WindowModel : ViewModel
{
    internal Window? InnerWindow { get; set; }

    public virtual void OnWindowInitialized(LightWindow window)
    {
    }
}
