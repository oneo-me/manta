namespace Axe.Reactive;

public abstract class ViewAttribute : Attribute
{
    protected internal abstract Type GetViewType();
}
