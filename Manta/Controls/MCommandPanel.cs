using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public class MCommandPanel : ListBox
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MCommandPanelItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MCommandPanelItem>(item, out recycleKey);
    }

    public static readonly StyledProperty<string> PlaceholderTextProperty = AvaloniaProperty.Register<MCommandPanel, string>(
        nameof(PlaceholderText));

    public string PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<MCommandPanel, string>(
        nameof(Text));

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}

public enum MCommandVariant
{
    Default,
    Separator,
    Group,
    NoResults
}

public class MCommandPanelItem : ListBoxItem
{
    public static readonly StyledProperty<MCommandVariant> VariantProperty = AvaloniaProperty.Register<MCommandPanelItem, MCommandVariant>(
        nameof(Variant));

    public MCommandVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
