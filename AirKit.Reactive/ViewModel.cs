using AirKit.Reactive.Controls;
using R3;

namespace AirKit.Reactive;

public abstract class ViewModel : Model
{
    internal LightView? InnerView { get; set; }

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
