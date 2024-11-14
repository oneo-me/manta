using System.ComponentModel;
using System.Reactive.Disposables;
using AirKit.Reactive.Controls;
using AirKit.Reactive.Extensions;

namespace AirKit.Reactive;

public abstract class DialogModel : ViewModel
{
    bool _autoSize = true;

    public bool AutoSize
    {
        get => _autoSize;
        set => SetValue(ref _autoSize, value);
    }

    bool _showBackground = true;

    public bool ShowBackground
    {
        get => _showBackground;
        set => SetValue(ref _showBackground, value);
    }

    string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => SetValue(ref _title, value);
    }

    internal Action<DialogModel>? CloseFunc { get; set; }

    public void Close()
    {
        if (CloseFunc is null)
            throw new InvalidOperationException($"{this} is not opened.");

        var cancelEventArgs = new CancelEventArgs();
        OnClosing(cancelEventArgs);

        if (cancelEventArgs.Cancel)
            return;

        CloseFunc(this);

        OnClosed();
    }

    protected virtual void OnClosing(CancelEventArgs e)
    {
    }

    protected virtual void OnClosed()
    {
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var window = this.GetWindow();
        window.FocusActivate();
    }
}
