using AirKit.Logging;
using Avalonia.Controls;

namespace AirKit;

public abstract class WindowModel : ViewModel
{
    internal Window? InnerWindow { get; set; }

    public virtual void OnWindowInitialized(Window window)
    {
        Log.Verbose("WindowModel", "{viewModel} initialized", this);
    }
}
