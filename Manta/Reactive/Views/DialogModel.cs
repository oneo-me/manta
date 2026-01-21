using Avalonia.Controls;
using Manta.Controls;

namespace Manta.Reactive.Views;

public abstract class DialogModel : WindowModel
{
    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);

        window.CanResize = false;
        window.CanMinimize = false;
        window.CanMaximize = false;
        window.ShowInTaskbar = false;
        window.SystemDecorations = SystemDecorations.BorderOnly;
        window.WindowStartupLocation = WindowStartupLocation.Manual;
        window.SizeToContent = SizeToContent.WidthAndHeight;
    }
}
