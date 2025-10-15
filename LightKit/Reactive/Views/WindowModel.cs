using Avalonia.Controls;

namespace LightKit.Reactive.Views;

public abstract class WindowModel : ViewModel
{
    internal Window? InnerWindow { get; set; }

    public virtual void OnWindowInitialized(Window window)
    {
    }
}
