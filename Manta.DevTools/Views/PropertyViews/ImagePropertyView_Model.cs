using Avalonia.Controls;
using Avalonia.Data;
using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<ImagePropertyView>]
sealed class ImagePropertyView_Model(ImagePropertyItem item) : ViewModel
{
    public static ImagePropertyView_Model Design => new(null!);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var view = this.GetView<ImageSourcePropertyView>();
        var image = view.Image;

        image[!Image.SourceProperty] = new Binding(item.Path)
        {
            Source = item.Source,
            Mode = BindingMode.OneWay
        };
    }
}
