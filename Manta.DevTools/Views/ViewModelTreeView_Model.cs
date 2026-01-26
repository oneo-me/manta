using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Collections;
using DynamicData.Binding;
using Manta.DevTools.Models;
using Manta.Logging;
using Manta.Reactive.Extensions;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

[View<ViewModelTreeView>]
sealed class ViewModelTreeView_Model : ViewModel
{
    public static ViewModelTreeView_Model Design => new();

    public ViewContext<AvaloniaList<TreeNode>> Root => GetContext<AvaloniaList<TreeNode>>(nameof(Root));
    public ViewContext<ReactiveValue<TreeNode?>> SelectedNode => GetContext<ReactiveValue<TreeNode?>>(nameof(SelectedNode));

    int _count;

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        _count = 0;

        if (SelectedNode.Value is null || Root.Value is null)
            return;

        SelectedNode.Value
            .WhenPropertyChanged(x => x.Value)
            .Where(x => x.Value is null)
            .Throttle(TimeSpan.FromMilliseconds(1000d / 60d))
            .ObserveOnDispatcher()
            .Subscribe(_ =>
            {
                _count++;

                Log.Debug("{0}", _count);

                var current = Root.Value.LastOrDefault();
                while (current is not null && current.Count > 0)
                    current = current.LastOrDefault();

                if (current is not null)
                    SelectedNode.Value.Value = current;
            })
            .DisposeWith(disposables);
    }
}
