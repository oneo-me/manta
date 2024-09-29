using Avalonia.Controls;

namespace AirKit.Reactive;

public abstract class WindowModel : ViewModel
{
    internal Window? InnerWindow { get; set; }

    public abstract void OnWindowInitialized(Window window);

    public void Close()
    {
        InnerWindow?.Close();
    }
}
