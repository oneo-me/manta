using Manta.Controls;
using Manta.Reactive.Views;

namespace Manta.DevTools.Models;

sealed class ViewNode : IDisposable
{
    public ViewNode? Parent { get; }
    public MView View { get; }
    public ViewModel ViewModel { get; }
    public ViewNodeContainer Container { get; }

    public string Name { get; }

    public ViewNode(ViewNode? parent, MView view, ViewModel viewModel)
    {
        Parent = parent;
        View = view;
        ViewModel = viewModel;
        Container = new(this, view);

        var viewModelType = viewModel.GetType();

        Name = viewModelType.Name;
    }

    public void Dispose()
    {
        Container.Dispose();
    }
}
