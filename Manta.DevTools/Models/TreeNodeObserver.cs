using System.Reactive.Disposables;
using Avalonia.Collections;
using Avalonia.LogicalTree;
using Manta.Controls;
using Manta.Reactive.Extensions;
using Manta.Reactive.Views;

namespace Manta.DevTools.Models;

sealed class TreeNodeObserver : IDisposable
{
    readonly TreeNode _node;
    readonly Action<MView, ViewModel> _onChildAdded;
    readonly Action<MView> _onChildRemoved;
    readonly CompositeDisposable _disposable = new();
    readonly Dictionary<ILogical, TreeNodeObserver> _childObservers = new();

    public TreeNodeObserver(TreeNode node, ILogical logical, Action<MView, ViewModel> onChildAdded, Action<MView> onChildRemoved)
    {
        _node = node;
        _onChildAdded = onChildAdded;
        _onChildRemoved = onChildRemoved;

        logical.LogicalChildren
            .ForEachItem(OnChildAdded, OnChildRemoved, OnChildrenReset)
            .DisposeWith(_disposable);
    }

    void OnChildAdded(ILogical item)
    {
        if (item is MView { DataContext: ViewModel viewModel } childView && viewModel != _node.ViewModel)
            _onChildAdded(childView, viewModel);
        else
            _childObservers.Add(item, new(_node, item, _onChildAdded, _onChildRemoved));
    }

    void OnChildRemoved(ILogical item)
    {
        if (item is MView { DataContext: ViewModel viewModel } childView && viewModel != _node.ViewModel)
        {
            _onChildRemoved(childView);
        }
        else if (_childObservers.TryGetValue(item, out var observer))
        {
            observer.Dispose();
            _childObservers.Remove(item);
        }
    }

    void OnChildrenReset()
    {
        var keys = _childObservers.Keys.ToList();

        foreach (var key in keys)
            OnChildRemoved(key);
    }

    public void Dispose()
    {
        _disposable.Dispose();

        foreach (var observer in _childObservers.Values)
            observer.Dispose();

        _childObservers.Clear();
    }
}
