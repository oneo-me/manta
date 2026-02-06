using System.Collections;
using Manta.DevTools.Models.Items;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<CollectionPropertyView>]
sealed class CollectionPropertyView_Model(CollectionPropertyItem item) : ViewModel
{
    public static CollectionPropertyView_Model Design => new(null!);
    public CollectionPropertyItem Item { get; } = item;
    public string TypeName => Item.PropertyType.Name;
    public string CountText => $"[{Item.Count}]";

    public CommandWarper OpenCommand => GetCommand(async () =>
    {
        if (Item.Value is not IEnumerable enumerable)
            return;

        var dialog = new CollectionPropertyDialog_Model
        {
            PropertyName = Item.Name,
            PropertyValue = enumerable
        };

        await dialog.ShowDialogAsync(this);
    });
}
