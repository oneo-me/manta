using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Manta.Controls;

public enum MExpanderType
{
    Single,
    Multiple
}

public class MExpander : ItemsControl
{
    public static readonly StyledProperty<MExpanderType> TypeProperty = AvaloniaProperty.Register<MExpander, MExpanderType>(
        nameof(Type));

    public MExpanderType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MExpanderItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MExpanderItem>(item, out recycleKey);
    }

    bool _ignored;

    public void CollapseOtherItems(MExpanderItem currentItem)
    {
        try
        {
            if (_ignored)
                return;

            _ignored = true;

            if (Type != MExpanderType.Single)
                return;

            foreach (var item in Items)
                if (item is MExpanderItem accordionItem && accordionItem != currentItem)
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
public class MExpanderItem : HeaderedContentControl
{
    public static readonly StyledProperty<bool> IsExpandedProperty = AvaloniaProperty.Register<MExpanderItem, bool>(
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
                this.GetLogicalParent<MExpander>()?.CollapseOtherItems(this);
        }
    }
}
