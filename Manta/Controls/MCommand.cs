using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public class MCommand : ListBox
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MCommandItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MCommandItem>(item, out recycleKey);
    }

    public static readonly StyledProperty<string> WatermarkProperty = AvaloniaProperty.Register<MCommand, string>(
        nameof(Watermark));

    public string Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<MCommand, string>(
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

public class MCommandItem : ListBoxItem
{
    public static readonly StyledProperty<MCommandVariant> VariantProperty = AvaloniaProperty.Register<MCommandItem, MCommandVariant>(
        nameof(Variant));

    public MCommandVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
