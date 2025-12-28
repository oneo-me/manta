using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Metadata;

namespace Manta.Controls;

public class MComboBox : ListBox
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MComboBoxItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MComboBoxItem>(item, out recycleKey);
    }

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty = AvaloniaProperty.Register<MComboBox, IBrush?>(
        nameof(PlaceholderForeground));

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> SelectedIteMemplateProperty = AvaloniaProperty.Register<MComboBox, IDataTemplate?>(
        nameof(SelectedIteMemplate));

    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IDataTemplate? SelectedIteMemplate
    {
        get => GetValue(SelectedIteMemplateProperty);
        set => SetValue(SelectedIteMemplateProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty = AvaloniaProperty.Register<MComboBox, HorizontalAlignment>(
        nameof(HorizontalContentAlignment));

    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }

    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty = AvaloniaProperty.Register<MComboBox, VerticalAlignment>(
        nameof(VerticalContentAlignment));

    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }

    public static readonly StyledProperty<bool> IsDropDownOpenProperty = AvaloniaProperty.Register<MComboBox, bool>(
        nameof(IsDropDownOpen));

    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public static readonly StyledProperty<double> MaxDropDownHeightProperty = AvaloniaProperty.Register<MComboBox, double>(
        nameof(MaxDropDownHeight));

    public double MaxDropDownHeight
    {
        get => GetValue(MaxDropDownHeightProperty);
        set => SetValue(MaxDropDownHeightProperty, value);
    }
}

public class MComboBoxItem : ComboBoxItem;
