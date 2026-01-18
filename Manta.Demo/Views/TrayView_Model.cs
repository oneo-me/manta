using Avalonia.Controls;
using Manta.Controls;
using Manta.Reactive.Views;

namespace Manta.Demo.Views;

[View<TrayView>]
sealed class TrayView_Model : TrayWindowModel
{
    public static TrayView_Model Design => new();

    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);

        window.Width = 280;
        window.SizeToContent = SizeToContent.Height;
    }

    public void ActiveApp()
    {
        MApp.Current.Activate();
    }

    public void Exit()
    {
        MApp.Current.Exit();
    }
}
