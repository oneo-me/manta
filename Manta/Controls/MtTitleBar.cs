using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Manta.Extensions;
using Manta.Reactive.Extensions;

namespace Manta.Controls;

[TemplatePart(nameof(PART_Event), typeof(Border))]
[TemplatePart(nameof(PART_Content), typeof(Panel))]
[TemplatePart(nameof(PART_Title), typeof(MtText))]
[TemplatePart(nameof(PART_Buttons), typeof(MtTitleBarButtons))]
public class MtTitleBar : ContentControl
{
    Border? PART_Event;
    Panel? PART_Content;
    MtText? PART_Title;
    MtTitleBarButtons? PART_Buttons;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Event = e.NameScope.Get<Border>(nameof(PART_Event));
        PART_Content = e.NameScope.Get<Panel>(nameof(PART_Content));
        PART_Title = e.NameScope.Get<MtText>(nameof(PART_Title));
        PART_Buttons = e.NameScope.Get<MtTitleBarButtons>(nameof(PART_Buttons));

        PART_Buttons.IsVisible = OperatingSystem.IsMacOS() is false;

        AttachEvents();
    }

    CompositeDisposable? _disposables;

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

    void AttachEvents()
    {
        if (PART_Event is null)
            return;

        if (VisualRoot is not Window window)
            return;

        _disposables?.Dispose();
        _disposables = null;

        _disposables = new();

        PART_Event
            .SubscribeTitleBarEvents(window, x => x)
            .DisposeWith(_disposables);

        PART_Event
            .SubscribeWindowStatus(window, x =>
            {
                if (OperatingSystem.IsMacOS() is false)
                    return;

                var isFull = x.isFullscreen || x.leftOffset <= 0;

                PART_Content?.Margin = isFull ? default : new Thickness(x.leftOffset, 0, 0, 0);
                PART_Title?.Margin = isFull ? default : new Thickness(-PART_Title?.Padding.Left ?? 0, 0, 0, 0);
            });

        window.GetObservable(Window.TitleProperty).Subscribe(x =>
        {
            PART_Title?.Text = x;
        }).DisposeWith(_disposables);
    }
}

[TemplatePart(nameof(PART_FullScreenButton), typeof(MtTitleBarButton))]
[TemplatePart(nameof(PART_MinimizeButton), typeof(MtTitleBarButton))]
[TemplatePart(nameof(PART_MaximizeRestoreButton), typeof(MtTitleBarButton))]
[TemplatePart(nameof(PART_CloseButton), typeof(MtTitleBarButton))]
public class MtTitleBarButtons : ContentControl
{
    MtTitleBarButton? PART_FullScreenButton;
    MtTitleBarButton? PART_MinimizeButton;
    MtTitleBarButton? PART_MaximizeRestoreButton;
    MtTitleBarButton? PART_CloseButton;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_FullScreenButton = e.NameScope.Get<MtTitleBarButton>(nameof(PART_FullScreenButton));
        PART_MinimizeButton = e.NameScope.Get<MtTitleBarButton>(nameof(PART_MinimizeButton));
        PART_MaximizeRestoreButton = e.NameScope.Get<MtTitleBarButton>(nameof(PART_MaximizeRestoreButton));
        PART_CloseButton = e.NameScope.Get<MtTitleBarButton>(nameof(PART_CloseButton));

        PART_FullScreenButton.Click += FullScreenButtonOnClick;
        PART_MinimizeButton.Click += MinimizeButtonOnClick;
        PART_MaximizeRestoreButton.Click += MaximizeRestoreButtonOnClick;
        PART_CloseButton.Click += CloseButtonOnClick;

        AttachEvents();
    }

    void FullScreenButtonOnClick(object? sender, RoutedEventArgs routedEventArgs)
    {
        if (VisualRoot is not Window window)
            return;

        window.WindowState = window.WindowState == WindowState.FullScreen ? WindowState.Normal : WindowState.FullScreen;
    }

    void MinimizeButtonOnClick(object? sender, RoutedEventArgs routedEventArgs)
    {
        if (VisualRoot is not Window window)
            return;

        window.WindowState = WindowState.Minimized;
    }

    void MaximizeRestoreButtonOnClick(object? sender, RoutedEventArgs routedEventArgs)
    {
        if (VisualRoot is not Window window)
            return;

        window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    void CloseButtonOnClick(object? sender, RoutedEventArgs e)
    {
        if (VisualRoot is not Window window)
            return;

        window.Close();
    }

    CompositeDisposable? _disposables;

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

    void AttachEvents()
    {
        if (VisualRoot is not Window window)
            return;

        _disposables?.Dispose();
        _disposables = null;

        _disposables = new();

        window.GetObservable(Window.ShowInTaskbarProperty).Subscribe(x =>
        {
            PART_MinimizeButton?.IsVisible = x;
        }).DisposeWith(_disposables);

        window.GetObservable(Window.CanResizeProperty).CombineLatest(window.GetObservable(MtWindow.AllowFullScreenProperty)).Subscribe(x =>
        {
            PART_MaximizeRestoreButton?.IsVisible = x.First;
            PART_FullScreenButton?.IsVisible = x is { First: true, Second: true };
        }).DisposeWith(_disposables);

        window.GetObservable(Window.WindowStateProperty).Subscribe(x =>
        {
            PART_MaximizeRestoreButton?.Type = x == WindowState.Maximized ? MtTitleBarButtonType.Restore : MtTitleBarButtonType.Maximize;
            PART_FullScreenButton?.Type = x == WindowState.FullScreen ? MtTitleBarButtonType.ExitFullScreen : MtTitleBarButtonType.FullScreen;
        }).DisposeWith(_disposables);
    }
}

public enum MtTitleBarButtonType
{
    None,

    Close,
    Minimize,
    Maximize,
    Restore,
    FullScreen,
    ExitFullScreen
}

public class MtTitleBarButton : Button
{
    static MtTitleBarButton()
    {
        AffectsRender<MtTitleBarButton>(IconDataProperty);
    }

    public static readonly StyledProperty<Geometry?> IconDataProperty = AvaloniaProperty.Register<MtTitleBarButton, Geometry?>(
        nameof(IconData));

    public Geometry? IconData
    {
        get => GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }

    public static readonly StyledProperty<MtTitleBarButtonType> TypeProperty = AvaloniaProperty.Register<MtTitleBarButton, MtTitleBarButtonType>(
        nameof(Type));

    public MtTitleBarButtonType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }
}
