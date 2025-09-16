namespace Axe.Reactive;

public abstract class RegionModel : ViewModel
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
    }

    public override string ToString()
    {
        return $"{GetType().Name} (Header: {Header})";
    }
}
