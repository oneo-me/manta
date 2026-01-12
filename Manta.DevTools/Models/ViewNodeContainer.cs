using System.Reactive.Disposables;
using Avalonia.Collections;
using Avalonia.LogicalTree;
using Manta.Controls;
using Manta.Reactive.Views;

namespace Manta.DevTools.Models;

sealed class ViewNodeContainer : IDisposable
{
    readonly CompositeDisposable _subscriptions = new();
    readonly Dictionary<MView, ILogical> _viewSources = new();

    public AvaloniaList<ViewNode> Children { get; } = [];

    public ViewNodeContainer(MView? view)
    {
        if (view is not ILogical logical)
            return;

        SubscribeToLogical(logical);
    }

    void SubscribeToLogical(ILogical logical)
    {
        var subscription = logical.LogicalChildren.ForEachItem(
            (_, item) => OnChildAdded(item, logical),
            (_, item) => OnChildRemoved(item, logical),
            () => OnChildrenReset(logical));

        _subscriptions.Add(subscription);
    }

    void OnChildAdded(ILogical item, ILogical source)
    {
        if (item is MView { DataContext: ViewModel viewModel } childView)
        {
            _viewSources[childView] = source;
            Children.Add(new(childView, viewModel));
        }
        else if (item is { } childLogical)
        {
            SubscribeToLogical(childLogical);
        }
    }

    void OnChildRemoved(ILogical item, ILogical source)
    {
        if (item is MView childView)
        {
            if (_viewSources.TryGetValue(childView, out var storedSource) && storedSource == source)
            {
                _viewSources.Remove(childView);
                for (var j = 0; j < Children.Count; j++)
                    if (Children[j].View == childView)
                    {
                        Children[j].Dispose();
                        Children.RemoveAt(j);
                        break;
                    }
            }
        }
        else if (item is { } childLogical)
        {
            RemoveViewsFromContainer(childLogical);
        }
    }

    void OnChildrenReset(ILogical source)
    {
        var toRemove = _viewSources
            .Where(kv => kv.Value == source)
            .Select(kv => kv.Key)
            .ToList();

        foreach (var view in toRemove)
        {
            _viewSources.Remove(view);
            for (var j = 0; j < Children.Count; j++)
                if (Children[j].View == view)
                {
                    Children[j].Dispose();
                    Children.RemoveAt(j);
                    break;
                }
        }
    }

    void RemoveViewsFromContainer(ILogical container)
    {
        foreach (var child in container.LogicalChildren)
            if (child is MView childView)
            {
                _viewSources.Remove(childView);
                for (var j = 0; j < Children.Count; j++)
                    if (Children[j].View == childView)
                    {
                        Children[j].Dispose();
                        Children.RemoveAt(j);
                        break;
                    }
            }
            else if (child is { } childLogical)
            {
                RemoveViewsFromContainer(childLogical);
            }
    }

    public void Dispose()
    {
        _subscriptions.Dispose();

        foreach (var child in Children)
            child.Dispose();

        Children.Clear();
        _viewSources.Clear();
    }
}
