using Avalonia.Controls;
using Avalonia.Data;
using Manta.DevTools.Models.Items;
using Manta.Globalization.Markups;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<StringPropertyView>]
sealed class StringPropertyView_Model(StringPropertyItem item) : ViewModel
{
    public static StringPropertyView_Model Design => new(null!);
    public StringPropertyItem Item { get; } = item;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        var view = this.GetView<StringPropertyView>();
        var bindingPath = Item.IsReactive ? $"{Item.Name}.Value" : Item.Name;

        if (Item.IsReadOnly)
        {
            view.EditBox.IsVisible = false;
            view.ReadOnlyText[!TextBlock.TextProperty] = new LangExtension(new Binding(bindingPath) { Source = Item.Source }).ToBinding();
        }
        else
        {
            view.ReadOnlyText.IsVisible = false;
            view.EditBox[!TextBox.TextProperty] = new LangExtension(new Binding(bindingPath, BindingMode.TwoWay) { Source = Item.Source }).ToBinding();
        }
    }
}
