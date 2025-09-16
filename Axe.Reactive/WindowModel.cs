using Avalonia.Controls;

namespace Axe.Reactive;

public abstract class WindowModel : ViewModel
{
    internal Window? InnerWindow { get; set; }

    public virtual void OnWindowInitialized(Window window)
    {
    }
}
