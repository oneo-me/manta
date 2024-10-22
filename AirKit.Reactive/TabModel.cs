using AirKit.Reactive.Logging;

namespace AirKit.Reactive;

public class TabModel : ViewModel
{
    string _header = string.Empty;

    public string Header
    {
        get => _header;
        set => SetValue(ref _header, value);
    }

    internal Action<TabModel>? CloseFunc { get; set; }

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
