namespace AirKit.Reactive.Attributes;

public abstract class ViewAttribute : Attribute
{
    protected internal abstract Type GetViewType();
}
