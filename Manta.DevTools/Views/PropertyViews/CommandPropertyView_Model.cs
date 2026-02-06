using Avalonia.Controls;
using Manta.DevTools.Models.Items;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views.PropertyViews;

[View<CommandPropertyView>]
sealed class CommandPropertyView_Model(CommandPropertyItem item) : ViewModel
{
    public static CommandPropertyView_Model Design => new(null!);
    public CommandPropertyItem Item { get; } = item;
    public bool CanExecute => !Item.HasParameter;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Item.HasParameter)
        {
            var view = this.GetView<CommandPropertyView>();
            ToolTip.SetTip(view.ExecuteButton, "Command requires parameter");
        }
    }
}
