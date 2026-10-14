using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Manta.Controls;

[TemplatePart(Name = "PART_Border", Type = typeof(Border))]
[TemplatePart(Name = "PART_Host", Type = typeof(Border))]
public class MClip : ContentControl
{
    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<MClip, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == BorderThicknessProperty || change.Property == CornerRadiusProperty)
            OnClipChanged();
    }

    Border? PART_Border;
    Border? PART_Host;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Border = e.NameScope.Get<Border>("PART_Border");
        PART_Host = e.NameScope.Get<Border>("PART_Host");

        OnClipChanged();
    }

    void OnClipChanged()
    {
        var size = CornerRadius.TopLeft;
        var border = BorderThickness.Left;

        if (PART_Border is not null)
        {
            var value = Math.Max(0, size - border / 2d);
            PART_Border.CornerRadius = new(value);
        }

        if (PART_Host is not null)
        {
            var value = Math.Max(0, size - border);
            PART_Host.CornerRadius = new(value);
        }
    }
}
