using System.Diagnostics.CodeAnalysis;
using AirKit.Reactive.Extensions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using R3;

namespace AirKit.Reactive.Controls;

[TemplatePart(nameof(PART_Title), typeof(TextBlock))]
[TemplatePart(nameof(PART_Events), typeof(Panel))]
[TemplatePart(nameof(PART_Offset), typeof(Border))]
[TemplatePart(nameof(PART_Buttons), typeof(Panel))]
[SuppressMessage("Design", "CA1001:具有可释放字段的类型应该是可释放的")]
public class LightTitleBar : ContentControl
{
    public static readonly StyledProperty<object?> InnerRightContentProperty = AvaloniaProperty.Register<LightTitleBar, object?>(
        nameof(InnerRightContent));

    public object? InnerRightContent
    {
        get => GetValue(InnerRightContentProperty);
        set => SetValue(InnerRightContentProperty, value);
    }

    public LightTitleBar()
    {
        PseudoClasses.Set(":empty", true);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ContentProperty)
            PseudoClasses.Set(":empty", change.NewValue == null);
        else if (change.Property == InnerRightContentProperty)
            LogicalChildren.ContentChanged(change);
    }

    TextBlock? PART_Title;
    Panel? PART_Events;
    Border? PART_Offset;
    Panel? PART_Buttons;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Title = e.NameScope.Get<TextBlock>(nameof(PART_Title));
        PART_Events = e.NameScope.Get<Panel>(nameof(PART_Events));
        PART_Offset = e.NameScope.Get<Border>(nameof(PART_Offset));
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
        _disposables?.Dispose();
        _disposables = null;

        if (VisualRoot is not Window window)
            return;

        _disposables = new();

        if (PART_Title is not null)
            window.GetObservable(Window.TitleProperty)
                .Subscribe(x =>
                {
                    PART_Title.Text = x;
                })
                .AddTo(_disposables);
        if (PART_Events is not null)
        {
            Observable
                .FromEventHandler<TappedEventArgs>(x => PART_Events.DoubleTapped += x, x => PART_Events.DoubleTapped -= x)
                .Subscribe(_ =>
                {
                    window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                })
                .AddTo(_disposables);
            Observable
                .FromEventHandler<PointerPressedEventArgs>(x => PART_Events.PointerPressed += x, x => PART_Events.PointerPressed -= x)
                .Subscribe(x =>
                {
                    window.BeginMoveDrag(x.e);
                })
                .AddTo(_disposables);
        }

        if (PART_Buttons is not null)
            foreach (var child in PART_Buttons.Children)
            {
                if (child is not LightTitleBarButton button)
                    continue;

                if (button.Type == LightTitleBarButtonType.Maximize)
                    window.GetObservable(Window.WindowStateProperty)
                        .Subscribe(x =>
                        {
                            button.Content = x == WindowState.Maximized;
                        })
                        .AddTo(_disposables);

                if (Design.IsDesignMode is false)
                    Observable
                        .FromEventHandler<RoutedEventArgs>(x => button.Click += x, x => button.Click -= x)
                        .Subscribe(x =>
                        {
                            if (x.sender is not LightTitleBarButton sender)
                                return;

                            switch (sender.Type)
                            {
                                case LightTitleBarButtonType.Minimize:
                                    window.WindowState = WindowState.Minimized;
                                    break;

                                case LightTitleBarButtonType.Maximize:
                                    window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                                    break;

                                case LightTitleBarButtonType.Close:
                                    window.Close();
                                    break;
                            }
                        })
                        .AddTo(_disposables);
            }
    }
}
