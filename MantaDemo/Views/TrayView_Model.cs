using Avalonia.Controls;
using Manta;
using Manta.Controls;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;

namespace MantaDemo.Views;

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

    public CommandWarper ActiveAppCommand => GetCommand(() =>
    {
        MApp.Current.Activate();
    });

    public CommandWarper ExitCommand => GetCommand(() =>
    {
        MApp.Current.Exit();
    });
}
