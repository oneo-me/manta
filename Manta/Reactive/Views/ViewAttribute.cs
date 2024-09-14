namespace Manta.Reactive.Views;

public abstract class ViewAttribute : Attribute
{
    protected internal abstract Type GetViewType();
}
