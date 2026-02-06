using Manta.DevTools.Models.Items;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<ReadOnlyPropertyView>]
sealed class ReadOnlyPropertyView_Model(ReadOnlyPropertyItem item) : ViewModel
{
    public static ReadOnlyPropertyView_Model Design => new(null!);
    public string DisplayValue { get; } = item.DisplayValue;
}
