using System.Collections;
using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

[View<CollectionPropertyDialog>]
sealed class CollectionPropertyDialog_Model(CollectionPropertyItem item) : DialogModel
{
    public string Header { get; } = item.Name;
    public IEnumerable? Items { get; } = item.GetValue() as IEnumerable;

    public object? SelectedItem
    {
        get;
        set => SetValue(ref field, value);
    }
}
