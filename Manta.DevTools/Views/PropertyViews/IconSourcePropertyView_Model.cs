using Avalonia.Data;
using Manta.Controls;
using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<IconSourcePropertyView>]
sealed class IconSourcePropertyView_Model(IconSourcePropertyItem item) : ViewModel
{
    public static IconSourcePropertyView_Model Design => new(null!);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var view = this.GetView<IconSourcePropertyView>();
        var icon = view.Icon;

        icon[!MIcon.IconSourceProperty] = new Binding(item.Path)
        {
            Source = item.Source,
            Mode = BindingMode.OneWay
        };
    }
}
