using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Threading;

namespace Manta.Controls;

sealed class MTextBoxCaret
{
    readonly TextBox _source;
    readonly Border PART_Caret;
    readonly TextPresenter PART_TextPresenter;

    public MTextBoxCaret(TextBox source, Border partCaret, TextPresenter partTextPresenter)
    {
        _source = source;
        PART_Caret = partCaret;
        PART_TextPresenter = partTextPresenter;

        // TextPresenter.PreeditTextProperty
        source.PropertyChanged += PartTextPresenterOnPropertyChanged;
        partTextPresenter.LayoutUpdated += OnTextPresenterLayoutUpdated;

        // Subscribe to preedit property changes for IME support
        partTextPresenter.GetObservable(TextPresenter.PreeditTextProperty).Subscribe(_ => OnPreeditChanged());
        partTextPresenter.GetObservable(TextPresenter.PreeditTextCursorPositionProperty).Subscribe(_ => OnPreeditChanged());

        UpdateCaretPosition(false);
    }

    void PartTextPresenterOnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == TextBox.CaretIndexProperty)
        {
            UpdateCaretPosition(true);
            ResetBlink();
        }
        else if (change.Property == TextBox.SelectionStartProperty ||
                 change.Property == TextBox.SelectionEndProperty)
        {
            UpdateCaretVisibility();
        }
        else if (change.Property == InputElement.IsFocusedProperty)
        {
            if (change.GetNewValue<bool>())
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

    void OnTextPresenterLayoutUpdated(object? sender, EventArgs e)
    {
        UpdateCaretPosition(!_isFirstUpdate);
        _isFirstUpdate = false;
    }

    void OnPreeditChanged()
    {
        UpdateCaretPosition(true);
        ResetBlink();
    }

    void UpdateCaretPosition(bool animate)
    {
        var textLayout = PART_TextPresenter.TextLayout;

        // Get caret position, considering preedit text (IME composition)
        var caretIndex = PART_TextPresenter.CaretIndex;

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
        var presenterPosition = PART_TextPresenter.TranslatePoint(new(0, 0), _source);
        if (presenterPosition is null) return;

        var x = hitTestResult.X + presenterPosition.Value.X;
        var y = hitTestResult.Y + presenterPosition.Value.Y;
        var height = hitTestResult.Height;

        // Ensure minimum height
        if (height < 1)
            height = PART_TextPresenter.FontSize > 0 ? PART_TextPresenter.FontSize * 1 : 12;

        _targetHeight = height * 0.8;
        // _targetX = x + 1; // Slight offset for better appearance
        _targetX = x;
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
        var progress = Math.Min(1.0, elapsed / 80d); // 80ms animation duration
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
                Interval = _source.CaretBlinkInterval
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

    void UpdateCaretVisibility()
    {
        // Hide caret when text is selected or not focused
        var hasSelection = _source.SelectionStart != _source.SelectionEnd;
        var shouldShow = _source.IsFocused && !hasSelection && _isCaretVisible;
        PART_Caret.Opacity = shouldShow ? 1 : 0;
    }
}
