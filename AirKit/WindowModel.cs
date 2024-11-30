using AirKit.Controls;
using AirKit.Logging;
using Avalonia.Controls;

namespace AirKit;

public abstract class WindowModel : ViewModel
{
    internal Window? InnerWindow { get; set; }

    public virtual void OnWindowInitialized(LightWindow window)
    {
        Log.Verbose("WindowModel", "{viewModel} initialized", this);

        window.Icon = LightApp.Current.Icon;
        window.Title = LightApp.Current.Name;
    }
}
