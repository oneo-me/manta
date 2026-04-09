using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using Manta.Controls;
using Manta.Reactive.Extensions;
using Observable = System.Reactive.Linq.Observable;

namespace Manta.Extensions;

public static class VisualExtensions
{
    static readonly SolidColorBrush _transparentBrush = new(Color.FromArgb(0, 0, 0, 0));

    public static void EnsureBackground(this InputElement inputElement)
    {
        if (Design.IsDesignMode)
            return;

        switch (inputElement)
        {
            case TemplatedControl templatedControl:
                templatedControl.Background ??= _transparentBrush;
                break;

            case Border border:
                border.Background ??= _transparentBrush;
                break;

            case Panel panel:
                panel.Background ??= _transparentBrush;
                break;
        }
    }

    public static IDisposable BindTitleBarEvents(this InputElement inputElement, Window? window = null, bool stateControl = true)
    {
        if (Design.IsDesignMode)
            return Disposable.Empty;

        inputElement.EnsureBackground();

        var disposables = new CompositeDisposable();

        if (stateControl)
            ObservableEvent.FromHandler<TappedEventArgs>(x => inputElement.DoubleTapped += x, x => inputElement.DoubleTapped -= x).Subscribe(x =>
                {
                    // Ignore if the event source is an interactive control
                    if (x.args.Source is Visual source && source.GetSelfAndVisualAncestors().Any(v => v is Button or MTabTitleBarItem))
                        return;

                    var currentWindow = GetWindow();
                    if (currentWindow is null)
                        return;

                    if (currentWindow.CanResize is false)
                        return;

                    if (OperatingSystem.IsMacOS() && currentWindow.WindowState == WindowState.FullScreen)
                        return;

                    currentWindow.WindowState = currentWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                })
                .DisposeWith(disposables);

        ObservableEvent.FromHandler<PointerPressedEventArgs>(x => inputElement.PointerPressed += x, x => inputElement.PointerPressed -= x).Subscribe(x =>
            {
                var currentWindow = GetWindow();
                if (currentWindow is null)
                    return;

                if (x.args.GetCurrentPoint(inputElement).Properties.IsLeftButtonPressed is false)
                    return;

                currentWindow.BeginMoveDrag(x.args);
            })
            .DisposeWith(disposables);

        return disposables;

        Window? GetWindow()
        {
            if (window is not null)
                return window;

            if (TopLevel.GetTopLevel(inputElement) is Window newWindow)
                return window = newWindow;

            return window;
        }
    }

    public static IObservable<WindowStatus> GetWindowStatusObservable(this MView view)
    {
        if (TopLevel.GetTopLevel(view) is not Window window)
            return Observable.Empty<WindowStatus>();

        return window.GetWindowStatusObservable();
    }
}
