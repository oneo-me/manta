using Manta.DevTools.Views.PropertyViews;
using Manta.Reactive.Views;

namespace Manta.DevTools.Models.PropertyItems;

sealed class NumericPropertyItem(string name, Type propertyType, object source, string path, bool isReadOnly, Func<object?> getValue) : PropertyItemBase(name, propertyType, source, path, isReadOnly, getValue)
{
    public override double Width => 160d;

    public override ViewModel CreateView()
    {
        return new NumericPropertyView_Model(this);
    }
}
