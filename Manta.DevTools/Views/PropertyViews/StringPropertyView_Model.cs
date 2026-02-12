using Avalonia.Controls;
using Avalonia.Data;
using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<StringPropertyView>]
sealed class StringPropertyView_Model(StringPropertyItem item) : ViewModel
{
    public static StringPropertyView_Model Design => new(null!);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var view = this.GetView<StringPropertyView>();
        var textBox = view.TextBox;

        textBox.IsReadOnly = item.IsReadOnly;
        textBox[!TextBox.TextProperty] = new Binding(item.Path)
        {
            Source = item.Source,
            Mode = item.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
        };
    }
}
