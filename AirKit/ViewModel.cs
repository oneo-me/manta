using System.Reactive.Disposables;
using AirKit.Controls;
using AirKit.Logging;

namespace AirKit;

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
        Log.Verbose("ViewModel", "{viewModel} initialized", this);
    }

    protected virtual void OnViewActivated(CompositeDisposable disposables)
    {
        Log.Verbose("ViewModel", "{viewModel} activated", this);
    }
}
