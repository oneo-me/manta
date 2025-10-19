using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using LightKit.Reactive.Extensions;

namespace LightKit.Controls;

[TemplatePart(nameof(PART_Buttons), typeof(Panel))]
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable")]
public class LightTitleBarButtons : ContentControl
{
    Panel? PART_Buttons;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Buttons = e.NameScope.Get<Panel>(nameof(PART_Buttons));

        AttachEvents();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        AttachEvents();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _disposables?.Dispose();
        _disposables = null;
    }

    CompositeDisposable? _disposables;

    void AttachEvents()
    {
        if (PART_Buttons is null)
            return;

        if (VisualRoot is not Window window)
            return;

        _disposables?.Dispose();
        _disposables = null;

        _disposables = new();

        foreach (var child in PART_Buttons.Children)
        {
            if (child is not LightTitleBarButton button)
                continue;

            if (button.Type == LightTitleBarButtonType.Minimize)
                window.GetObservable(Window.ShowInTaskbarProperty).Subscribe(x =>
                    {
                        button.IsVisible = x;
                    })
                    .DisposeWith(_disposables);

            if (button.Type == LightTitleBarButtonType.Maximize)
            {
                window.GetObservable(Window.CanResizeProperty).Subscribe(x =>
                    {
                        button.IsVisible = x;
                    })
                    .DisposeWith(_disposables);
                window.GetObservable(Window.WindowStateProperty).Subscribe(x =>
                    {
                        button.Content = x == WindowState.Maximized;
                    })
                    .DisposeWith(_disposables);
            }

            if (Design.IsDesignMode)
                continue;

            Observable
                .FromEventHandler<RoutedEventArgs>(x => button.Click += x, x => button.Click -= x).Subscribe(x =>
                {
                    if (x.sender is not LightTitleBarButton sender)
                        return;

                    switch (sender.Type)
                    {
                        case LightTitleBarButtonType.Minimize:
                            if (OperatingSystem.IsMacOS() && window.WindowState == WindowState.FullScreen)
                                return;

                            window.WindowState = WindowState.Minimized;
                            break;

                        case LightTitleBarButtonType.Maximize:
                            if (OperatingSystem.IsMacOS() && window.WindowState == WindowState.FullScreen)
                                return;

                            if (window.CanResize)
                                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                            break;

                        case LightTitleBarButtonType.Close:
                            window.Close();
                            break;
                    }
                })
                .DisposeWith(_disposables);
        }
    }
}
