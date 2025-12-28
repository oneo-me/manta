using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Manta.Controls;

public enum LightAccordionType
{
    Single,
    Multiple
}

public class LightAccordion : ItemsControl
{
    public static readonly StyledProperty<LightAccordionType> TypeProperty = AvaloniaProperty.Register<LightAccordion, LightAccordionType>(
        nameof(Type));

    public LightAccordionType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new LightAccordionItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<LightAccordionItem>(item, out recycleKey);
    }

    bool _ignored;

    public void CollapseOtherItems(LightAccordionItem currentItem)
    {
        try
        {
            if (_ignored)
                return;

            _ignored = true;

            if (Type != LightAccordionType.Single)
                return;

            foreach (var item in Items)
                if (item is LightAccordionItem accordionItem && accordionItem != currentItem)
                    accordionItem.IsExpanded = false;
        }
        finally
        {
            _ignored = false;
        }
    }
}

[PseudoClasses(":expanded")]
[TemplatePart("PART_Header", typeof(Border))]
public class LightAccordionItem : HeaderedContentControl
{
    public static readonly StyledProperty<bool> IsExpandedProperty = AvaloniaProperty.Register<LightAccordionItem, bool>(
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
            var newValue = change.NewValue is true;

            PseudoClasses.Set(":expanded", newValue);

            if (newValue)
                this.GetLogicalParent<LightAccordion>()?.CollapseOtherItems(this);
        }
    }
}
