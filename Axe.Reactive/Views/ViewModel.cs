using System.Reactive.Disposables;
using Axe.Reactive.Controls;

namespace Axe.Reactive.Views;

public abstract class ViewModel : Model
{
    internal View? InnerView { get; set; }

    bool _isInitialized;

    internal void OnInnerViewActivated(CompositeDisposable disposables)
    {
        if (_isInitialized is false)
        {
            _isInitialized = true;
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
