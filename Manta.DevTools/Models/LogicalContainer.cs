using System.Reactive.Disposables;
using Avalonia.Collections;
using Avalonia.LogicalTree;
using Manta.Reactive.Extensions;

namespace Manta.DevTools.Models;

sealed class LogicalContainer : IDisposable
{
    readonly CompositeDisposable _disposable = new();

    public AvaloniaList<LogicalNode> Children { get; } = [];

    public LogicalContainer(LogicalNode parent, ILogical logical)
    {
        logical.LogicalChildren.ForEachItem((i, item) =>
        {
            Children.Insert(i, new(parent, item));
        }, (i, _) =>
        {
            Children.RemoveAt(i);
        }, () =>
        {
            for (var i = Children.Count - 1; i >= 0; i--)
            {
                var item = Children[i];
                Children.RemoveAt(i);
                item.Dispose();
            }
        }).DisposeWith(_disposable);
    }

    public void Dispose()
    {
        _disposable.Dispose();

        for (var i = Children.Count - 1; i >= 0; i--)
        {
            var item = Children[i];
            Children.RemoveAt(i);
            item.Dispose();
        }
    }
}
