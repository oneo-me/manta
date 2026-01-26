using Avalonia.Collections;
using Manta.Controls;
using Manta.Controls.Icons;
using Manta.Reactive.Regions;
using Manta.Reactive.Views;

namespace Manta.DevTools.Models;

sealed partial class TreeNode : AvaloniaList<TreeNode>, IDisposable
{
    readonly TreeNodeObserver _treeNodeObserver;

    public TreeNode? Parent { get; }
    public MView View { get; }
    public ViewModel ViewModel { get; }

    public IconSource Icon { get; }
    public string Name { get; }

    public TreeNode(TreeNode? parent, MView view, ViewModel viewModel)
    {
        Parent = parent;
        View = view;
        ViewModel = viewModel;

        var viewModelType = viewModel.GetType();

        Name = viewModelType.Name;
        Icon = viewModel switch
        {
            DialogModel => Icons.DialogModelIcon,
            WindowModel => Icons.WindowModelIcon,
            RegionModel => Icons.RegionModelIcon,
            _           => Icons.ViewModelIcon
        };

        _treeNodeObserver = new(this, view, OnChildAdded, OnChildRemoved);
    }

    void OnChildAdded(MView mView, ViewModel viewModel)
    {
        Add(new(this, mView, viewModel));
    }

    void OnChildRemoved(MView mView)
    {
        for (var i = 0; i < Count; i++)
            if (this[i].View == mView)
            {
                this[i].Dispose();
                RemoveAt(i);
                break;
            }
    }

    public void Dispose()
    {
        _treeNodeObserver.Dispose();
        ClearAdorner();

        foreach (var child in this)
            child.Dispose();

        Clear();
    }
}
