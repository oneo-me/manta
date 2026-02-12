using Manta.Reactive.Views;

namespace Manta.DevTools.Models.PropertyItems;

sealed class GroupHeaderItem(string name) : ItemBase(name, null!)
{
    public override ViewModel CreateView()
    {
        throw new NotImplementedException();
    }
}
