using Avalonia.Controls;
using Avalonia.Data;
using Manta.DevTools.Models.PropertyItems;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<CommandPropertyView>]
sealed class CommandPropertyView_Model(CommandPropertyItem item) : ViewModel
{
    public static CommandPropertyView_Model Design => new(null!);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var view = this.GetView<CommandPropertyView>();
        var button = view.Button;

        button[!Button.CommandProperty] = new Binding(item.Path)
        {
            Source = item.Source,
            Mode = item.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
        };
    }
}
