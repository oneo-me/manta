using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

[View<ObjectPropertyDialog>]
sealed class ObjectPropertyDialog_Model(ObjectPropertyItem item) : DialogModel
{
    public string Header { get; } = item.Name;
    public object? Value { get; } = item.GetValue();
}
