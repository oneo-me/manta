using Avalonia.Controls;
using Avalonia.Data;
using Manta.DevTools.Models.Items;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<NumericPropertyView>]
sealed class NumericPropertyView_Model(NumericPropertyItem item) : ViewModel
{
    public static NumericPropertyView_Model Design => new(null!);
    public NumericPropertyItem Item { get; } = item;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        var view = this.GetView<NumericPropertyView>();
        var bindingPath = Item.IsReactive ? $"{Item.Name}.Value" : Item.Name;

        if (Item.IsReadOnly)
        {
            view.EditBox.IsVisible = false;
            view.ReadOnlyText[!TextBlock.TextProperty] = new Binding(bindingPath) { Source = Item.Source };
        }
        else
        {
            view.ReadOnlyText.IsVisible = false;
            view.EditBox[!TextBox.TextProperty] = new Binding(bindingPath, BindingMode.TwoWay) { Source = Item.Source };
        }
    }
}
