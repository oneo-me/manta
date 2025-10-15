using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Metadata;
using LightKit.Controls.Icons;
using LightKit.Controls.Icons.Inners;
using Path = Avalonia.Controls.Shapes.Path;

namespace LightKit.Controls;

public sealed class LightIcon : TemplatedControl
{
    public static readonly StyledProperty<double?> IconSizeProperty = AvaloniaProperty.Register<LightIcon, double?>(
        nameof(IconSize));

    public double? IconSize
    {
        get => GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public static readonly StyledProperty<IconSource?> IconSourceProperty = AvaloniaProperty.Register<LightIcon, IconSource?>(
        nameof(IconSource));

    public IconSource? IconSource
    {
        get => GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    public static readonly StyledProperty<IconStyle> IconStyleProperty = AvaloniaProperty.Register<LightIcon, IconStyle>(
        nameof(IconStyle));

    [DependsOn(nameof(IconSource))]
    public IconStyle IconStyle
    {
        get => GetValue(IconStyleProperty);
        set => SetValue(IconStyleProperty, value);
    }

    Viewbox? PART_ViewBox;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_ViewBox = e.NameScope.Get<Viewbox>(nameof(PART_ViewBox));
        PART_ViewBox.Child = _panel;

        OnIconSizeChanged(IconSize);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IconSourceProperty)
            OnSourceChanged(change.NewValue as IconSource);
        else if (change.Property == IconSizeProperty)
            OnIconSizeChanged(change.NewValue is double value ? value : 0);
    }

    void OnIconSizeChanged(double? size)
    {
        var viewBox = PART_ViewBox;
        if (viewBox is null)
            return;

        viewBox.Width = viewBox.Height = size ?? double.NaN;
    }

    readonly Panel _panel = new();

    void OnSourceChanged(IconSource? source)
    {
        if (source is null)
            return;

        _panel.Width = source.Width;
        _panel.Height = source.Height;

        foreach (var layer in source.Layers)
        {
            Shape? shape = null;

            if (layer is IconPathLayer pathLayer)
                shape = new Path
                {
                    Data = pathLayer.Data,
                    StrokeLineCap = pathLayer.StrokeLineCap,
                    StrokeThickness = pathLayer.StrokeWidth
                };

            if (shape is null)
                continue;

            switch (IconStyle)
            {
                case IconStyle.Default:
                    if (string.IsNullOrEmpty(layer.Stroke) is false)
                    {
                        shape[!Shape.FillProperty] = this[!BackgroundProperty];
                        shape[!Shape.StrokeProperty] = this[!ForegroundProperty];
                    }
                    else
                    {
                        shape[!Shape.FillProperty] = this[!ForegroundProperty];
                    }

                    break;

                case IconStyle.OnlyBorder:
                    if (string.IsNullOrEmpty(layer.Stroke) is false)
                        shape[!Shape.StrokeProperty] = this[!ForegroundProperty];
                    else
                        shape[!Shape.FillProperty] = this[!ForegroundProperty];
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _panel.Children.Add(shape);
        }
    }
}
