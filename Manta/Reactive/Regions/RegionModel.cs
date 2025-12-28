using Manta.Reactive.Views;

namespace Manta.Reactive.Regions;

public abstract class RegionModel : ViewModel
{
    internal Action<RegionModel>? CloseFunc { get; set; }

    public void Close()
    {
        CloseFunc?.Invoke(this);
        OnClosed();
    }

    protected virtual void OnClosed()
    {
    }
}
