using Manta.Controls;
using Manta.Reactive.Views;

namespace Manta.DevTools.Models;

sealed class ViewNode : IDisposable
{
    public MView View { get; }
    public ViewModel ViewModel { get; }
    public ViewNodeContainer Container { get; }

    public string Name { get; }

    public ViewNode(MView view, ViewModel viewModel)
    {
        View = view;
        ViewModel = viewModel;
        Container = new(view);

        var viewModelType = viewModel.GetType();

        Name = viewModelType.Name;
    }

    public void Dispose()
    {
        Container.Dispose();
    }
}
