using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<EnumPropertyView>]
sealed class EnumPropertyView_Model(EnumPropertyItem item) : ViewModel
{
    public static EnumPropertyView_Model Design => new(null!);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var view = this.GetView<EnumPropertyView>();
        var textBox = view.TextBox;
        var comboBox = view.ComboBox;

        if (item.IsReadOnly)
        {
            comboBox.IsVisible = false;
            textBox[!TextBox.TextProperty] = new Binding(item.Path)
            {
                Source = item.Source,
                Mode = BindingMode.OneWay
            };
        }
        else
        {
            textBox.IsVisible = false;
            comboBox.ItemsSource = item.PropertyType.GetEnumValues();
            comboBox[!SelectingItemsControl.SelectedItemProperty] = new Binding(item.Path)
            {
                Source = item.Source,
                Mode = BindingMode.TwoWay
            };
        }
    }
}
