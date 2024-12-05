using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using AirKit.Extensions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace AirKit.Controls;

[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable")]
[TemplatePart(nameof(PART_Events), typeof(Panel))]
public class TitleBarExt : ContentControl
{
    public static readonly StyledProperty<string?> TitleProperty = AvaloniaProperty.Register<TitleBarExt, string?>(
        nameof(Title));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<bool> UseWindowTitleProperty = AvaloniaProperty.Register<TitleBarExt, bool>(
        nameof(UseWindowTitle), true);

    public bool UseWindowTitle
    {
        get => GetValue(UseWindowTitleProperty);
        set => SetValue(UseWindowTitleProperty, value);
    }

    public static readonly StyledProperty<bool> IsShowTitleProperty = AvaloniaProperty.Register<TitleBarExt, bool>(
        nameof(IsShowTitle), true);

    public bool IsShowTitle
    {
        get => GetValue(IsShowTitleProperty);
        set => SetValue(IsShowTitleProperty, value);
    }

    public static readonly StyledProperty<bool> IsShowButtonsProperty = AvaloniaProperty.Register<TitleBarExt, bool>(
        nameof(IsShowButtons), true);

    public bool IsShowButtons
    {
        get => GetValue(IsShowButtonsProperty);
        set => SetValue(IsShowButtonsProperty, value);
    }

    Panel? PART_Events;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Events = e.NameScope.Get<Panel>(nameof(PART_Events));
        AttachEvents();
    }

    bool _isAttached;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        _isAttached = true;
        AttachEvents();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _isAttached = false;
        _disposables?.Dispose();
        _disposables = null;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == UseWindowTitleProperty)
            AttachEvents();
    }

    CompositeDisposable? _disposables;

    void AttachEvents()
    {
        _disposables?.Dispose();
        _disposables = null;

        if (_isAttached is false)
            return;

        if (VisualRoot is not Window window)
            return;

        _disposables = new();

        PART_Events?.SubscribeTitleBarEvents(x => x)
            .DisposeWith(_disposables);

        if (UseWindowTitle)
            window
                .GetObservable(Window.TitleProperty)
                .Subscribe(x =>
                {
                    Title = x;
                })
                .DisposeWith(_disposables);
    }
}
