using Manta.Controls;

namespace Manta.Reactive.Views;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ViewAttribute<T> : ViewAttribute where T : LightView, new()
{
    protected internal override Type GetViewType()
    {
        return typeof(T);
    }
}
