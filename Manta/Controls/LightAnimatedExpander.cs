using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Threading;

namespace Manta.Controls;

/// <summary>
/// A container that animates height changes when expanding/collapsing its content.
/// This solves Avalonia's limitation of not supporting transitions for Auto-sized values.
/// </summary>
public class LightAnimatedExpander : Decorator
{
    public static readonly StyledProperty<bool> IsExpandedProperty = AvaloniaProperty.Register<LightAnimatedExpander, bool>(
        nameof(IsExpanded));

    public static readonly StyledProperty<TimeSpan> DurationProperty = AvaloniaProperty.Register<LightAnimatedExpander, TimeSpan>(
        nameof(Duration), TimeSpan.FromMilliseconds(200));

    public static readonly StyledProperty<Easing> EasingProperty = AvaloniaProperty.Register<LightAnimatedExpander, Easing>(
        nameof(Easing), new CubicEaseOut());

    public bool IsExpanded
    {
        get => GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public TimeSpan Duration
    {
        get => GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    public Easing Easing
    {
        get => GetValue(EasingProperty);
        set => SetValue(EasingProperty, value);
    }

    double _targetHeight;
    double _currentAnimatedHeight;
    double _startHeight;
    double _endHeight;
    long _animationStartTicks;
    DispatcherTimer? _animationTimer;
    bool _isFirstLayout = true;
    bool _isAnimating;

    public LightAnimatedExpander()
    {
        ClipToBounds = true;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsExpandedProperty)
        {
            var isExpanded = change.NewValue is true;
            StartAnimation(isExpanded);
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        if (Child is null)
            return default;

        // Always measure child with infinite height to get its natural size
        Child.Measure(new Size(availableSize.Width, double.PositiveInfinity));
        _targetHeight = Child.DesiredSize.Height;

        if (_isFirstLayout)
        {
            _isFirstLayout = false;
            _currentAnimatedHeight = IsExpanded ? _targetHeight : 0;
        }

        // When expanded and not animating, follow content's natural height
        var height = (_isAnimating || !IsExpanded) ? _currentAnimatedHeight : _targetHeight;
        return new Size(Child.DesiredSize.Width, height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (Child is null)
            return default;

        // Arrange child at its natural size
        Child.Arrange(new Rect(0, 0, finalSize.Width, _targetHeight));

        // When expanded and not animating, follow content's natural height
        var height = (_isAnimating || !IsExpanded) ? _currentAnimatedHeight : _targetHeight;
        return new Size(finalSize.Width, height);
    }

    void StartAnimation(bool expand)
    {
        // Ensure we have measured the child
        if (Child is not null)
        {
            Child.Measure(new Size(Bounds.Width > 0 ? Bounds.Width : double.PositiveInfinity, double.PositiveInfinity));
            _targetHeight = Child.DesiredSize.Height;
        }

        _startHeight = _currentAnimatedHeight;
        _endHeight = expand ? _targetHeight : 0;

        if (Math.Abs(_startHeight - _endHeight) < 0.1)
        {
            _currentAnimatedHeight = _endHeight;
            InvalidateMeasure();
            return;
        }

        _animationStartTicks = Environment.TickCount64;
        _isAnimating = true;

        if (_animationTimer is null)
        {
            _animationTimer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(8) // ~120fps for smooth animation
            };
            _animationTimer.Tick += OnAnimationTick;
        }

        _animationTimer.Start();
    }

    void OnAnimationTick(object? sender, EventArgs e)
    {
        var elapsed = Environment.TickCount64 - _animationStartTicks;
        var progress = Math.Min(1.0, elapsed / Duration.TotalMilliseconds);
        var easedProgress = Easing.Ease(progress);

        _currentAnimatedHeight = _startHeight + (_endHeight - _startHeight) * easedProgress;
        InvalidateMeasure();

        if (progress >= 1.0)
        {
            _animationTimer?.Stop();
            _isAnimating = false;
            _currentAnimatedHeight = _endHeight;
            InvalidateMeasure();
        }
    }
}
