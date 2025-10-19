using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Metadata;

namespace LightKit.Controls;

public class LightProgressRing : ProgressBar
{
    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<LightProgressRing, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }

    public static readonly StyledProperty<IBrush?> RingBackgroundProperty =
        AvaloniaProperty.Register<LightProgressRing, IBrush?>(nameof(RingBackground));

    public IBrush? RingBackground
    {
        get => GetValue(RingBackgroundProperty);
        set => SetValue(RingBackgroundProperty, value);
    }

    public static readonly StyledProperty<IBrush?> RingForegroundProperty =
        AvaloniaProperty.Register<LightProgressRing, IBrush?>(nameof(RingForeground));

    public IBrush? RingForeground
    {
        get => GetValue(RingForegroundProperty);
        set => SetValue(RingForegroundProperty, value);
    }

    public static readonly StyledProperty<double> RingSizeProperty = AvaloniaProperty.Register<LightProgressRing, double>(
        nameof(RingSize));

    public double RingSize
    {
        get => GetValue(RingSizeProperty);
        set => SetValue(RingSizeProperty, value);
    }

    public static readonly StyledProperty<float> RingStrokeSizeProperty =
        AvaloniaProperty.Register<LightProgressRing, float>(nameof(RingStrokeSize));

    public float RingStrokeSize
    {
        get => GetValue(RingStrokeSizeProperty);
        set => SetValue(RingStrokeSizeProperty, value);
    }

    public static readonly StyledProperty<object?> ContentProperty = AvaloniaProperty.Register<LightProgressRing, object?>(
        nameof(Content));

    [Content]
    [DependsOn(nameof(ContentTemplate))]
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> ContentTemplateProperty = AvaloniaProperty.Register<LightProgressRing, IDataTemplate?>(
        nameof(ContentTemplate));

    public IDataTemplate? ContentTemplate
    {
        get => GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty = AvaloniaProperty.Register<LightProgressRing, HorizontalAlignment>(
        nameof(HorizontalContentAlignment));

    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }

    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty = AvaloniaProperty.Register<LightProgressRing, VerticalAlignment>(
        nameof(VerticalContentAlignment));

    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ContentControl.ContentProperty)
            ContentChanged(change);
    }

    void ContentChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ILogical oldValue)
            LogicalChildren.Remove(oldValue);

        if (e.NewValue is not ILogical newValue)
            return;

        LogicalChildren.Add(newValue);
    }
}
