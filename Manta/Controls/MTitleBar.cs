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
[TemplatePart(nameof(PART_Spacing), typeof(Border))]
[TemplatePart(nameof(PART_Title), typeof(TextBlock))]
[TemplatePart(nameof(PART_Buttons), typeof(MTitleBarButtons))]
public class MTitleBar : ContentControl
{
    public static readonly StyledProperty<bool> ShowContentBorderProperty = AvaloniaProperty.Register<MTitleBar, bool>(
        nameof(ShowContentBorder), true);

    public bool ShowContentBorder
    {
        get => GetValue(ShowContentBorderProperty);
        set => SetValue(ShowContentBorderProperty, value);
    }

    Border? PART_Event;
    Border? PART_Spacing;
    TextBlock? PART_Title;
    MTitleBarButtons? PART_Buttons;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Event = e.NameScope.Get<Border>(nameof(PART_Event));
        PART_Spacing = e.NameScope.Get<Border>(nameof(PART_Spacing));
        PART_Title = e.NameScope.Get<TextBlock>(nameof(PART_Title));
        PART_Buttons = e.NameScope.Get<MTitleBarButtons>(nameof(PART_Buttons));

        PART_Buttons.IsVisible = Design.IsDesignMode || OperatingSystem.IsMacOS() is false;

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

        if (VisualRoot?.Parent is not Window window)
            return;

        _disposables?.Dispose();
        _disposables = null;

        _disposables = new();

        if (Design.IsDesignMode is false)
            PART_Event
                .BindTitleBarEvents(window)
                .DisposeWith(_disposables);

        if (OperatingSystem.IsMacOS())
            window.GetWindowStatusObservable()
                .Subscribe(x =>
                {
                    var isFull = x.IsFullScreen || x.Offset.Left <= 0;

                    PART_Spacing?.Width = isFull ? 0d : x.Offset.Left;
                    PART_Title?.Margin = isFull ? default : new Thickness(-PART_Title?.Padding.Left ?? 0, 0, 0, 0);
                })
                .DisposeWith(_disposables);

        window.GetObservable(Window.TitleProperty).Subscribe(x =>
        {
            PART_Title?.Text = x;
        }).DisposeWith(_disposables);
    }
}

[TemplatePart(nameof(PART_FullScreenButton), typeof(MTitleBarButton))]
[TemplatePart(nameof(PART_MinimizeButton), typeof(MTitleBarButton))]
[TemplatePart(nameof(PART_MaximizeRestoreButton), typeof(MTitleBarButton))]
[TemplatePart(nameof(PART_CloseButton), typeof(MTitleBarButton))]
public class MTitleBarButtons : ContentControl
{
    MTitleBarButton? PART_FullScreenButton;
    MTitleBarButton? PART_MinimizeButton;
    MTitleBarButton? PART_MaximizeRestoreButton;
    MTitleBarButton? PART_CloseButton;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_FullScreenButton = e.NameScope.Get<MTitleBarButton>(nameof(PART_FullScreenButton));
        PART_MinimizeButton = e.NameScope.Get<MTitleBarButton>(nameof(PART_MinimizeButton));
        PART_MaximizeRestoreButton = e.NameScope.Get<MTitleBarButton>(nameof(PART_MaximizeRestoreButton));
        PART_CloseButton = e.NameScope.Get<MTitleBarButton>(nameof(PART_CloseButton));

        if (Design.IsDesignMode is false)
        {
            PART_FullScreenButton.Click += FullScreenButtonOnClick;
            PART_MinimizeButton.Click += MinimizeButtonOnClick;
            PART_MaximizeRestoreButton.Click += MaximizeRestoreButtonOnClick;
            PART_CloseButton.Click += CloseButtonOnClick;
        }

        AttachEvents();
    }

    void FullScreenButtonOnClick(object? sender, RoutedEventArgs routedEventArgs)
    {
        if (VisualRoot?.Parent is not Window window)
            return;

        window.WindowState = window.WindowState == WindowState.FullScreen ? WindowState.Normal : WindowState.FullScreen;
    }

    void MinimizeButtonOnClick(object? sender, RoutedEventArgs routedEventArgs)
    {
        if (VisualRoot?.Parent is not Window window)
            return;

        window.WindowState = WindowState.Minimized;
    }

    void MaximizeRestoreButtonOnClick(object? sender, RoutedEventArgs routedEventArgs)
    {
        if (VisualRoot?.Parent is not Window window)
            return;

        window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    void CloseButtonOnClick(object? sender, RoutedEventArgs e)
    {
        if (VisualRoot?.Parent is not Window window)
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
        if (VisualRoot?.Parent is not Window window)
            return;

        _disposables?.Dispose();
        _disposables = null;

        _disposables = new();

        window.GetObservable(Window.ShowInTaskbarProperty).Subscribe(x =>
        {
            PART_MinimizeButton?.IsVisible = x;
        }).DisposeWith(_disposables);

        window.GetObservable(Window.CanResizeProperty).CombineLatest(window.GetObservable(MWindow.AllowFullScreenProperty)).Subscribe(x =>
        {
            PART_MaximizeRestoreButton?.IsVisible = x.First;
            PART_FullScreenButton?.IsVisible = x is { First: true, Second: true };
        }).DisposeWith(_disposables);

        window.GetObservable(Window.WindowStateProperty).Subscribe(x =>
        {
            PART_MaximizeRestoreButton?.Type = x == WindowState.Maximized ? MTitleBarButtonType.Restore : MTitleBarButtonType.Maximize;
            PART_FullScreenButton?.Type = x == WindowState.FullScreen ? MTitleBarButtonType.ExitFullScreen : MTitleBarButtonType.FullScreen;
        }).DisposeWith(_disposables);
    }
}

public enum MTitleBarButtonType
{
    None,

    Close,
    Minimize,
    Maximize,
    Restore,
    FullScreen,
    ExitFullScreen
}

public class MTitleBarButton : Button
{
    static MTitleBarButton()
    {
        AffectsRender<MTitleBarButton>(IconDataProperty);
    }

    public static readonly StyledProperty<Geometry?> IconDataProperty = AvaloniaProperty.Register<MTitleBarButton, Geometry?>(
        nameof(IconData));

    public Geometry? IconData
    {
        get => GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }

    public static readonly StyledProperty<MTitleBarButtonType> TypeProperty = AvaloniaProperty.Register<MTitleBarButton, MTitleBarButtonType>(
        nameof(Type));

    public MTitleBarButtonType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }
}
