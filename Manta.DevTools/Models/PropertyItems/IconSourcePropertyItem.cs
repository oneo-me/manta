using Manta.DevTools.Views.PropertyViews;
using Manta.Reactive.Views;

namespace Manta.DevTools.Models.PropertyItems;

sealed class IconSourcePropertyItem(string name, Type propertyType, object source, string path, bool isReadOnly, Func<object?> getValue) : PropertyItemBase(name, propertyType, source, path, isReadOnly, getValue)
{
    public override double Height => double.NaN;

    public override ViewModel CreateView()
    {
        return new IconSourcePropertyView_Model(this);
    }
}
