using AirKit.Reactive.Controls;

namespace AirKit.Reactive.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ViewAttribute<T> : ViewAttribute where T : LightView, new()
{
    protected internal override Type GetViewType()
    {
        return typeof(T);
    }
}
