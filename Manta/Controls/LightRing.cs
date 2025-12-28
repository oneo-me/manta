using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;

namespace Manta.Controls;

[TemplatePart(nameof(PART_Arc), typeof(Arc))]
public class LightRing : TemplatedControl
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<LightRing, double>(nameof(Value));

    [SuppressMessage("Naming", "CA1721:Property names should not match get methods")]
    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<LightRing, double>(nameof(Maximum));

    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly StyledProperty<double> SizeProperty = AvaloniaProperty.Register<LightRing, double>(
        nameof(Size));

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public static readonly StyledProperty<float> StrokeSizeProperty =
        AvaloniaProperty.Register<LightRing, float>(nameof(StrokeSize));

    public float StrokeSize
    {
        get => GetValue(StrokeSizeProperty);
        set => SetValue(StrokeSizeProperty, value);
    }

    Arc? PART_Arc;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Arc = e.NameScope.Get<Arc>(nameof(PART_Arc));
        Update();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ValueProperty || change.Property == MaximumProperty)
            Update();
    }

    void Update()
    {
        if (PART_Arc == null)
            return;

        PART_Arc.SweepAngle = Value / Maximum * 360;
    }
}
