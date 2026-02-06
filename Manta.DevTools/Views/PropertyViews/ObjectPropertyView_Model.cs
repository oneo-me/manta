using Manta.DevTools.Models.Items;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<ObjectPropertyView>]
sealed class ObjectPropertyView_Model(ObjectPropertyItem item) : ViewModel
{
    public static ObjectPropertyView_Model Design => new(null!);
    public ObjectPropertyItem Item { get; } = item;
    public string TypeName => Item.PropertyType.Name;

    public CommandWarper OpenCommand => GetCommand(async () =>
    {
        var dialog = new ObjectPropertyDialog_Model
        {
            PropertyName = Item.Name,
            PropertyValue = Item.Value
        };

        await dialog.ShowDialogAsync(this);
    });
}
