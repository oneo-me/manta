using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<BooleanPropertyView>]
sealed class BooleanPropertyView_Model(BooleanPropertyItem item) : ViewModel
{
    public static BooleanPropertyView_Model Design => new(null!);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var view = this.GetView<BooleanPropertyView>();
        var checkBox = view.CheckBox;

        checkBox.IsEnabled = item.IsReadOnly is false;
        checkBox[!ToggleButton.IsCheckedProperty] = new Binding(item.Path)
        {
            Source = item.Source,
            Mode = item.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
        };
    }
}
