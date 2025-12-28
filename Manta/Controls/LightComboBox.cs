using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Metadata;

namespace Manta.Controls;

public class LightComboBox : ListBox
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new LightComboBoxItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<LightComboBoxItem>(item, out recycleKey);
    }

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty = AvaloniaProperty.Register<LightComboBox, IBrush?>(
        nameof(PlaceholderForeground));

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> SelectedItemTemplateProperty = AvaloniaProperty.Register<LightComboBox, IDataTemplate?>(
        nameof(SelectedItemTemplate));

    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IDataTemplate? SelectedItemTemplate
    {
        get => GetValue(SelectedItemTemplateProperty);
        set => SetValue(SelectedItemTemplateProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty = AvaloniaProperty.Register<LightComboBox, HorizontalAlignment>(
        nameof(HorizontalContentAlignment));

    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }

    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty = AvaloniaProperty.Register<LightComboBox, VerticalAlignment>(
        nameof(VerticalContentAlignment));

    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }

    public static readonly StyledProperty<bool> IsDropDownOpenProperty = AvaloniaProperty.Register<LightComboBox, bool>(
        nameof(IsDropDownOpen));

    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public static readonly StyledProperty<double> MaxDropDownHeightProperty = AvaloniaProperty.Register<LightComboBox, double>(
        nameof(MaxDropDownHeight));

    public double MaxDropDownHeight
    {
        get => GetValue(MaxDropDownHeightProperty);
        set => SetValue(MaxDropDownHeightProperty, value);
    }
}

public class LightComboBoxItem : ComboBoxItem;
