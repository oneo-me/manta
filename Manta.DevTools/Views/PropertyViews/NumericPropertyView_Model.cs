using Avalonia.Controls;
using Avalonia.Data;
using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<NumericPropertyView>]
sealed class NumericPropertyView_Model(NumericPropertyItem item) : ViewModel
{
    public static NumericPropertyView_Model Design => new(null!);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var view = this.GetView<NumericPropertyView>();
        var numericUpDown = view.NumericUpDown;

        numericUpDown.IsReadOnly = item.IsReadOnly;
        numericUpDown[!NumericUpDown.ValueProperty] = new Binding(item.Path)
        {
            Source = item.Source,
            Mode = item.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
        };
    }
}
