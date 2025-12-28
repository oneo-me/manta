using System.Reactive.Disposables;
using Avalonia.Controls;
using Manta.Controls;

namespace Manta.Reactive.Views;

public abstract class ViewModel : Model
{
    protected static bool IsDesignMode { get; } = Design.IsDesignMode;

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
