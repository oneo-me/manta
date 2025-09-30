using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Metadata;
using Axe.Icons.Inners;
using Path = Avalonia.Controls.Shapes.Path;

namespace Axe.Icons;

public sealed class IconControl : TemplatedControl
{
    public static readonly StyledProperty<IconSource?> SourceProperty = AvaloniaProperty.Register<IconControl, IconSource?>(
        nameof(Source));

    public IconSource? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly StyledProperty<IconStyle> IconStyleProperty = AvaloniaProperty.Register<IconControl, IconStyle>(
        nameof(IconStyle));

    [DependsOn(nameof(Source))]
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
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SourceProperty)
            OnSourceChanged(change.NewValue as IconSource);
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
                    shape[!Shape.FillProperty] = this[!BackgroundProperty];
                    shape[!Shape.StrokeProperty] = this[!ForegroundProperty];
                    break;

                case IconStyle.OnlyBorder:
                    shape[!Shape.StrokeProperty] = this[!ForegroundProperty];
                    break;

                case IconStyle.OnlyFill:
                    shape[!Shape.FillProperty] = this[!BackgroundProperty];
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _panel.Children.Add(shape);
        }
    }
}
