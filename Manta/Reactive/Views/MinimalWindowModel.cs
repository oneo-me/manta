using Avalonia.Controls;
using Manta.Controls;

namespace Manta.Reactive.Views;

public abstract class MinimalWindowModel : WindowModel
{
    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);

        window.CanResize = false;
        window.CanMaximize = false;
        window.SizeToContent = SizeToContent.WidthAndHeight;
    }
}
