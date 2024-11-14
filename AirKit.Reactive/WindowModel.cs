using AirKit.Reactive.Logging;
using Avalonia.Controls;

namespace AirKit.Reactive;

public abstract class WindowModel : ViewModel
{
    internal Window? InnerWindow { get; set; }

    public virtual void OnWindowInitialized(Window window)
    {
        Log.Verbose("WindowModel", "{viewModel} initialized", this);
    }
}
