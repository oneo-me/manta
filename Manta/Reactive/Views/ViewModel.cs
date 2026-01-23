using System.Reactive.Disposables;
using Avalonia.Controls;
using Manta.Controls;

namespace Manta.Reactive.Views;

public abstract partial class ViewModel : CommandModel
{
    protected static bool IsDesignMode { get; } = Design.IsDesignMode;

    internal MView? InnerView { get; set; }

    bool _isInitialized;

    internal void OnInnerViewActivated(CompositeDisposable disposables)
    {
        if (_isInitialized is false)
        {
            _isInitialized = true;
            OnLoadViewContexts();
            OnInitialized();
        }

        OnViewActivated(disposables);
    }

    protected virtual void OnInitialized()
    {
    }

    protected virtual void OnViewActivated(CompositeDisposable disposables)
    {
    }
}
