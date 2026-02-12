using Manta.DevTools.Views.PropertyViews;
using Manta.Reactive.Views;

namespace Manta.DevTools.Models.PropertyItems;

sealed class ObjectPropertyItem(string name, Type propertyType, object source, string path, bool isReadOnly, Func<object?> getValue) : PropertyItemBase(name, propertyType, source, path, isReadOnly, getValue)
{
    public override ViewModel CreateView()
    {
        return new ObjectPropertyView_Model(this);
    }
}
