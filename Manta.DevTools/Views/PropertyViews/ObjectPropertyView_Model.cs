using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<ObjectPropertyView>]
sealed class ObjectPropertyView_Model(ObjectPropertyItem item) : ViewModel
{
    public static ObjectPropertyView_Model Design => new(null!);

    public CommandWarper OpenCommand => GetCommand(async () =>
    {
        var dialog = new ObjectPropertyDialog_Model(item);
        await dialog.ShowDialogAsync(this);
    });
}
