using Avalonia.Collections;
using Manta.DevTools.Models;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

[View<ViewModelTreeView>]
sealed class ViewModelTreeView_Model : ViewModel
{
    public static ViewModelTreeView_Model Design => new();

    public ViewContext<AvaloniaList<TreeNode>> Root => GetContext<AvaloniaList<TreeNode>>(nameof(Root));
    public ViewContext<ReactiveValue<TreeNode?>> SelectedNode => GetContext<ReactiveValue<TreeNode?>>(nameof(SelectedNode));
}
