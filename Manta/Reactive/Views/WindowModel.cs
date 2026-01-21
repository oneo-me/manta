using Manta.Controls;

namespace Manta.Reactive.Views;

public abstract class WindowModel : ViewModel
{
    internal MWindow? InnerWindow { get; set; }

    public virtual void OnWindowInitialized(MWindow window)
    {
    }

    public void Close()
    {
        this.GetWindowCore().Close();
    }

    public async Task CloseAsync()
    {
        if (UIThread.CheckAccess())
            Close();
        else
            await UIThread.InvokeAsync(Close);
    }
}
