using System.Reactive.Disposables;
using System.Reactive.Linq;
using AirKit.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace AirKit.Extensions;

public static class VisualExtensions
{
    public static IDisposable SubscribeWindowStatus<T>(this T view, Action<(bool isFullscreen, bool isShowButtons, double leftOffset, double topOffset)> action) where T : LightView
    {
        if (Design.IsDesignMode)
        {
            action((false, true, 0, 0));
            return Disposable.Empty;
        }

        if (view.GetVisualRoot() is not Window window)
            return Disposable.Empty;

        var isMacos = OperatingSystem.IsMacOS();

        return window.GetObservable(Window.WindowStateProperty)
            .StartWith(window.WindowState)
            .Subscribe(x =>
            {
                var isShowButtons = x is not WindowState.FullScreen;
                var leftOffset = isShowButtons ? 78 : 0;
                var topOffset = isShowButtons ? 38 : 0;

                action((isShowButtons is false, isShowButtons && isMacos is false, isMacos ? leftOffset : 0, isMacos ? topOffset : 0));
            });
    }

    public static IDisposable SubscribeTitleBarEvents<T>(this T view, Func<T, InputElement> func) where T : LightView
    {
        if (Design.IsDesignMode)
            return Disposable.Empty;

        if (view.GetVisualRoot() is not Window window)
            return Disposable.Empty;

        var inputElement = func(view);
        var disposables = new CompositeDisposable();

        switch (inputElement)
        {
            case TemplatedControl templatedControl:
                templatedControl.Background ??= new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                break;

            case Border border:
                border.Background ??= new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                break;

            case Panel panel:
                panel.Background ??= new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                break;
        }

        Observable.FromEventHandler<TappedEventArgs>(x => inputElement.DoubleTapped += x, x => inputElement.DoubleTapped -= x).Subscribe(_ =>
            {
                if (window.CanResize is false)
                    return;

                if (OperatingSystem.IsMacOS() && window.WindowState == WindowState.FullScreen)
                    return;

                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            })
            .DisposeWith(disposables);

        Observable.FromEventHandler<PointerPressedEventArgs>(x => inputElement.PointerPressed += x, x => inputElement.PointerPressed -= x).Subscribe(x =>
            {
                window.BeginMoveDrag(x.args);
            })
            .DisposeWith(disposables);

        return disposables;
    }
}
