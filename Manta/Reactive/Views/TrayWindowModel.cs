using Avalonia.Controls;
using Manta.Controls;

namespace Manta.Reactive.Views;

public abstract class TrayWindowModel : WindowModel
{
    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);

        window.Variant = MWindowVariant.TaryWindow;
        window.CanResize = false;
        window.CanMinimize = false;
        window.CanMaximize = false;
        window.ShowInTaskbar = false;
        window.CloseWhenDeactivated = true;
        window.SystemDecorations = SystemDecorations.BorderOnly;
        window.WindowStartupLocation = WindowStartupLocation.Manual;
        window.SizeToContent = SizeToContent.WidthAndHeight;
    }
}
