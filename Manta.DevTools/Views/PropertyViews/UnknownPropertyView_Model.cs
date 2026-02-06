using Manta.DevTools.Models.Items;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<UnknownPropertyView>]
sealed class UnknownPropertyView_Model(PropertyItemBase item) : ViewModel
{
    public static UnknownPropertyView_Model Design => new(null!);
    public string Message { get; } = $"Unknown type {item.PropertyType.FullName}";
}
