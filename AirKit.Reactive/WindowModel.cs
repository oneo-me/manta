using Avalonia.Controls;

namespace AirKit.Reactive;

public abstract class WindowModel : ViewModel
{
    public abstract void OnWindowInitialized(Window window);
}
