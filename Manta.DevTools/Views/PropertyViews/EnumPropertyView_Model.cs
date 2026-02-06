using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Manta.DevTools.Models.Items;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<EnumPropertyView>]
sealed class EnumPropertyView_Model(EnumPropertyItem item) : ViewModel
{
    public static EnumPropertyView_Model Design => new(null!);
    public EnumPropertyItem Item { get; } = item;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        var view = this.GetView<EnumPropertyView>();
        var bindingPath = Item.IsReactive ? $"{Item.Name}.Value" : Item.Name;

        if (Item.IsReadOnly)
        {
            view.EditBox.IsVisible = false;
            view.ReadOnlyText[!TextBlock.TextProperty] = new Binding(bindingPath) { Source = Item.Source };
        }
        else
        {
            view.ReadOnlyText.IsVisible = false;
            view.EditBox.ItemsSource = Item.EnumValues;
            view.EditBox[!SelectingItemsControl.SelectedItemProperty] = new Binding(bindingPath, BindingMode.TwoWay) { Source = Item.Source };
        }
    }
}
