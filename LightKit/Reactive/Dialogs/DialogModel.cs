using Avalonia.Controls;
using LightKit.Reactive.Views;

namespace LightKit.Reactive.Dialogs;

public abstract class DialogModel : WindowModel
{
    public override void OnWindowInitialized(Window window)
    {
        base.OnWindowInitialized(window);

        window.SizeToContent = SizeToContent.WidthAndHeight;
        window.SystemDecorations = SystemDecorations.BorderOnly;
        window.CanMinimize = false;
        window.CanMaximize = false;
        window.CanResize = false;
        window.ShowInTaskbar = false;
    }
}
