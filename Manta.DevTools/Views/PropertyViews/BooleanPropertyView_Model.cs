using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Manta.DevTools.Models.Items;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<BooleanPropertyView>]
sealed class BooleanPropertyView_Model(BooleanPropertyItem item) : ViewModel
{
    public static BooleanPropertyView_Model Design => new(null!);
    public BooleanPropertyItem Item { get; } = item;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var view = this.GetView<BooleanPropertyView>();
        var bindingPath = Item.IsReactive ? $"{Item.Name}.Value" : Item.Name;
        view.CheckBox.IsEnabled = !Item.IsReadOnly;
        view.CheckBox[!ToggleButton.IsCheckedProperty] = new Binding(bindingPath, BindingMode.TwoWay)
        {
            Source = Item.Source
        };
    }
}
