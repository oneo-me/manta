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
        window.Height = 420;
    }
}
