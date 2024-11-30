namespace AirKit.Attributes;

public abstract class ViewAttribute : Attribute
{
    protected internal abstract Type GetViewType();
}
