using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<CollectionPropertyView>]
sealed class CollectionPropertyView_Model(CollectionPropertyItem item) : ViewModel
{
    public static CollectionPropertyView_Model Design => new(null!);

    public CommandWarper OpenCommand => GetCommand(async () =>
    {
        var dialog = new CollectionPropertyDialog_Model(item);
        await dialog.ShowDialogAsync(this);
    });
}
