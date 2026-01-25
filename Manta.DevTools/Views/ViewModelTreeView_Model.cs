using Avalonia.Collections;
using Manta.DevTools.Models;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

[View<ViewModelTreeView>]
sealed class ViewModelTreeView_Model : ViewModel
{
    public static ViewModelTreeView_Model Design => new();

    public ViewContext<AvaloniaList<ViewNode>> Root => GetContext<AvaloniaList<ViewNode>>(nameof(Root));
    public ViewContext<ReactiveValue<ViewNode?>> SelectedNode => GetContext<ReactiveValue<ViewNode?>>(nameof(SelectedNode));
}
