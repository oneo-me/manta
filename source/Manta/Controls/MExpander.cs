using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Manta.Controls;

[PseudoClasses(":expanded")]
[TemplatePart("PART_Header", typeof(Border))]
public class MExpander : HeaderedContentControl
{
    public static readonly StyledProperty<bool> IsExpandedProperty = AvaloniaProperty.Register<MExpander, bool>(
        nameof(IsExpanded));

    public bool IsExpanded
    {
        get => GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    Border? PART_Header;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Header = e.NameScope.Get<Border>(nameof(PART_Header));
        PART_Header.PointerReleased += HeaderOnPointerReleased;
    }

    void HeaderOnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        IsExpanded = !IsExpanded;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsExpandedProperty)
        {
            var isExpanded = change.GetNewValue<bool>();
            PseudoClasses.Set(":expanded", isExpanded);
        }
    }
}
