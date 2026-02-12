using Avalonia.Data;
using Manta.Controls;
using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<ImageSourcePropertyView>]
sealed class ImageSourcePropertyView_Model(ImageSourcePropertyItem item) : ViewModel
{
    public static ImageSourcePropertyView_Model Design => new(null!);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var view = this.GetView<ImageSourcePropertyView>();
        var image = view.Image;

        image[!MImage.SourceProperty] = new Binding(item.Path)
        {
            Source = item.Source,
            Mode = BindingMode.OneWay
        };
    }
}
