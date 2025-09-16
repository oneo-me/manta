using Axe.Reactive.Controls;

namespace Axe.Reactive;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ViewAttribute<T> : ViewAttribute where T : View, new()
{
    protected internal override Type GetViewType()
    {
        return typeof(T);
    }
}
