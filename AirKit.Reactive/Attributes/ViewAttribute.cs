using System.Reflection;

namespace AirKit.Reactive.Attributes;

public abstract class ViewAttribute : Attribute
{
    protected internal abstract Type GetViewType();

    internal static ViewAttribute? Find(Type viewModelType)
    {
        if (viewModelType.GetCustomAttribute(typeof(ViewAttribute)) is ViewAttribute viewAttribute)
            return viewAttribute;

        return null;
    }
}
