using AirKit.Logging;

namespace AirKit;

public class RegionModel : ViewModel
{
    public string Header
    {
        get;
        set => SetValue(ref field, value);
    } = string.Empty;

    internal Action<RegionModel>? CloseFunc { get; set; }

    public void Close()
    {
        CloseFunc?.Invoke(this);
        OnClosed();
    }

    protected virtual void OnClosed()
    {
        Log.Verbose("TabModel", "TabModel Closed: {this}", this);
    }

    public override string ToString()
    {
        return $"{GetType().Name} (Header: {Header})";
    }
}
