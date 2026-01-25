using Avalonia.Controls;
using Manta.Controls;
using Manta.Reactive.Views;

namespace Manta.DevTools.Models;

sealed class ViewNode : NodeBase
{
    public ViewNode? Parent { get; }
    public MView View { get; }
    public ViewModel ViewModel { get; }
    public ViewNodeContainer Container { get; }

    public ViewNode(ViewNode? parent, MView view, ViewModel viewModel)
    {
        Parent = parent;
        View = view;
        ViewModel = viewModel;
        Container = new(this, view);

        var viewModelType = viewModel.GetType();

        Header = viewModelType.Name;
    }

    public void UpdateAdorner(IResourceHost resourceHost)
    {
        UpdateAdornerCore(View, resourceHost, true);
    }

    protected override void DisposeOverride()
    {
        Container.Dispose();
    }
}
