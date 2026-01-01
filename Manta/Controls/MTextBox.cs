using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Threading;

namespace Manta.Controls;

public enum MTextBoxVariant
{
    Default,
    Embedded
}

[TemplatePart(nameof(PART_Caret), typeof(Border))]
[TemplatePart(nameof(PART_TextPresenter), typeof(TextPresenter))]
public class MTextBox : TextBox
{
    public static readonly StyledProperty<MTextBoxVariant> VariantProperty = AvaloniaProperty.Register<MTextBox, MTextBoxVariant>(
        nameof(Variant));

    public MTextBoxVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<bool> IsPasswordBoxProperty = AvaloniaProperty.Register<MTextBox, bool>(
        nameof(IsPasswordBox));

    public bool IsPasswordBox
    {
        get => GetValue(IsPasswordBoxProperty);
        set => SetValue(IsPasswordBoxProperty, value);
    }

    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<MTextBox, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }

    public static readonly StyledProperty<TimeSpan> CaretAnimationDurationProperty = AvaloniaProperty.Register<MTextBox, TimeSpan>(
        nameof(CaretAnimationDuration), TimeSpan.FromMilliseconds(80));

    public TimeSpan CaretAnimationDuration
    {
        get => GetValue(CaretAnimationDurationProperty);
        set => SetValue(CaretAnimationDurationProperty, value);
    }

    Border? PART_Caret;
    TextPresenter? PART_TextPresenter;

    // Position animation state
    double _currentX;
    double _currentY;
    double _currentHeight;
    double _targetX;
    double _targetY;
    double _targetHeight;
    double _startX;
    double _startY;
    double _startHeight;
    long _animationStartTicks;
    DispatcherTimer? _animationTimer;
    bool _isFirstUpdate = true;
    readonly Easing _easing = new CubicEaseOut();

    // Blink state
    DispatcherTimer? _blinkTimer;
    bool _isCaretVisible = true;
    long _focusGainedTime; // Timestamp when focus was gained, to skip animation briefly

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Caret = e.NameScope.Get<Border>(nameof(PART_Caret));
        PART_TextPresenter = e.NameScope.Get<TextPresenter>(nameof(PART_TextPresenter));

        // TextPresenter.PreeditTextProperty
        PART_TextPresenter.LayoutUpdated += OnTextPresenterLayoutUpdated;

        // Subscribe to preedit property changes for IME support
        PART_TextPresenter.GetObservable(TextPresenter.PreeditTextProperty).Subscribe(_ => OnPreeditChanged());
        PART_TextPresenter.GetObservable(TextPresenter.PreeditTextCursorPositionProperty).Subscribe(_ => OnPreeditChanged());

        UpdateCaretPosition(false);
    }

    void OnPreeditChanged()
    {
        UpdateCaretPosition(true);
        ResetBlink();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == CaretIndexProperty)
        {
            UpdateCaretPosition(true);
            ResetBlink();
        }
        else if (change.Property == SelectionStartProperty ||
                 change.Property == SelectionEndProperty)
        {
            UpdateCaretVisibility();
        }
        else if (change.Property == IsFocusedProperty)
        {
            if (IsFocused)
            {
                _focusGainedTime = Environment.TickCount64;
                UpdateCaretPosition(false);
                StartBlink();
            }
            else
            {
                UpdateCaretVisibility();
                StopBlink();
            }
        }
    }

    void OnTextPresenterLayoutUpdated(object? sender, EventArgs e)
    {
        UpdateCaretPosition(!_isFirstUpdate);
        _isFirstUpdate = false;
    }

    void UpdateCaretVisibility()
    {
        if (PART_Caret is null) return;

        // Hide caret when text is selected or not focused
        var hasSelection = SelectionStart != SelectionEnd;
        var shouldShow = IsFocused && !hasSelection && _isCaretVisible;
        PART_Caret.Opacity = shouldShow ? 1 : 0;
    }

    void UpdateCaretPosition(bool animate)
    {
        if (PART_Caret is null || PART_TextPresenter is null) return;

        var textLayout = PART_TextPresenter.TextLayout;

        // Get caret position, considering preedit text (IME composition)
        var caretIndex = CaretIndex;

        // Check for preedit text (IME input)
        var preeditText = PART_TextPresenter.PreeditText;
        if (!string.IsNullOrEmpty(preeditText))
        {
            // Get preedit cursor position within the preedit text
            var preeditCursorPos = PART_TextPresenter.PreeditTextCursorPosition ?? preeditText.Length;
            caretIndex += preeditCursorPos;
        }

        var hitTestResult = textLayout.HitTestTextPosition(caretIndex);

        // Get position relative to TextPresenter, then transform to Canvas coordinate space
        var presenterPosition = PART_TextPresenter.TranslatePoint(new(0, 0), this);
        if (presenterPosition is null) return;

        var x = hitTestResult.X + presenterPosition.Value.X;
        var y = hitTestResult.Y + presenterPosition.Value.Y;
        var height = hitTestResult.Height;

        // Ensure minimum height
        if (height < 1)
            height = FontSize > 0 ? FontSize * 1 : 12;

        _targetHeight = height * 0.8;
        _targetX = x + 1;
        _targetY = y + (height - _targetHeight) / 2;

        // Skip animation for 100ms after gaining focus
        var timeSinceFocus = Environment.TickCount64 - _focusGainedTime;
        var shouldAnimate = animate && !_isFirstUpdate && timeSinceFocus > 100;

        if (shouldAnimate)
        {
            StartAnimation();
        }
        else
        {
            // Immediate update (no animation)
            _currentX = _targetX;
            _currentY = _targetY;
            _currentHeight = _targetHeight;
            ApplyCaretTransform();
        }

        UpdateCaretVisibility();
    }

    void StartAnimation()
    {
        // If already at target, skip animation
        if (Math.Abs(_currentX - _targetX) < 0.1 &&
            Math.Abs(_currentY - _targetY) < 0.1 &&
            Math.Abs(_currentHeight - _targetHeight) < 0.1)
            return;

        _startX = _currentX;
        _startY = _currentY;
        _startHeight = _currentHeight;
        _animationStartTicks = Environment.TickCount64;

        if (_animationTimer is null)
        {
            _animationTimer = new(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(8) // ~120fps
            };
            _animationTimer.Tick += OnAnimationTick;
        }

        _animationTimer.Start();
    }

    void OnAnimationTick(object? sender, EventArgs e)
    {
        var elapsed = Environment.TickCount64 - _animationStartTicks;
        var progress = Math.Min(1.0, elapsed / CaretAnimationDuration.TotalMilliseconds);
        var easedProgress = _easing.Ease(progress);

        _currentX = _startX + (_targetX - _startX) * easedProgress;
        _currentY = _startY + (_targetY - _startY) * easedProgress;
        _currentHeight = _startHeight + (_targetHeight - _startHeight) * easedProgress;

        ApplyCaretTransform();

        if (progress >= 1.0)
        {
            _animationTimer?.Stop();
            _currentX = _targetX;
            _currentY = _targetY;
            _currentHeight = _targetHeight;
            ApplyCaretTransform();
        }
    }

    void ApplyCaretTransform()
    {
        if (PART_Caret is null) return;

        Canvas.SetLeft(PART_Caret, _currentX);
        Canvas.SetTop(PART_Caret, _currentY);
        PART_Caret.Height = _currentHeight;
    }

    void StartBlink()
    {
        _isCaretVisible = true;
        UpdateCaretVisibility();

        if (_blinkTimer is null)
        {
            _blinkTimer = new()
            {
                Interval = CaretBlinkInterval
            };
            _blinkTimer.Tick += OnBlinkTick;
        }

        _blinkTimer.Start();
    }

    void StopBlink()
    {
        _blinkTimer?.Stop();
        _isCaretVisible = true;
        UpdateCaretVisibility();
    }

    void ResetBlink()
    {
        // Reset blink cycle when caret moves - show caret immediately
        _isCaretVisible = true;
        UpdateCaretVisibility();

        // Restart the timer
        _blinkTimer?.Stop();
        _blinkTimer?.Start();
    }

    void OnBlinkTick(object? sender, EventArgs e)
    {
        _isCaretVisible = !_isCaretVisible;
        UpdateCaretVisibility();
    }
}
