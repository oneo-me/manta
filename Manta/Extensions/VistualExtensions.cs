using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using Manta.Reactive.Extensions;
using Observable = Manta.Reactive.Extensions.Observable;

namespace Manta.Extensions;

public static class VisualExtensions
{
    extension<T>(T view) where T : InputElement
    {
        public IDisposable SubscribeWindowStatus(Window window, Action<(bool isFullscreen, bool isShowButtons, double leftOffset, double topOffset, double titleBarHeight)> action)
        {
            if (Design.IsDesignMode)
            {
                action((false, true, 0, 0, 38));
                return Disposable.Empty;
            }

            var isMacos = OperatingSystem.IsMacOS();

            return window.GetObservable(Window.WindowStateProperty)
                .StartWith(window.WindowState)
                .Subscribe(x =>
                {
                    var isShowButtons = x is not WindowState.FullScreen;
                    var leftOffset = isShowButtons ? 78 : 0;
                    var topOffset = isShowButtons ? 38 : 0;

                    action((isShowButtons is false, isShowButtons && isMacos is false, isMacos ? leftOffset : 0, isMacos ? topOffset : 0, 38));
                });
        }

        public IDisposable SubscribeWindowStatus(Action<(bool isFullscreen, bool isShowButtons, double leftOffset, double topOffset, double titleBarHeight)> action)
        {
            if (Design.IsDesignMode)
            {
                action((false, true, 0, 0, 38));
                return Disposable.Empty;
            }

            if (view.GetVisualRoot() is not Window window)
                return Disposable.Empty;

            return view.SubscribeWindowStatus(window, action);
        }

        public IDisposable SubscribeTitleBarEvents(Func<T, InputElement> func)
        {
            if (Design.IsDesignMode)
                return Disposable.Empty;

            if (view.GetVisualRoot() is not Window window)
                return Disposable.Empty;

            return view.SubscribeTitleBarEvents(window, func);
        }

        public IDisposable SubscribeTitleBarEvents(Window window, Func<T, InputElement> func)
        {
            if (Design.IsDesignMode)
                return Disposable.Empty;

            var inputElement = func(view);
            inputElement.EnsureBackground();

            var disposables = new CompositeDisposable();

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

    static readonly SolidColorBrush _transparentBrush = new(Color.FromArgb(0, 0, 0, 0));

    public static void EnsureBackground(this InputElement inputElement)
    {
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
}
