using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public class MtCommand : ListBox
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MtCommandItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MtCommandItem>(item, out recycleKey);
    }

    public static readonly StyledProperty<string> WatermarkProperty = AvaloniaProperty.Register<MtCommand, string>(
        nameof(Watermark));

    public string Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<MtCommand, string>(
        nameof(Text));

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}

public enum MtCommandVariant
{
    Default,
    Separator,
    Group,
    NoResults
}

public class MtCommandItem : ListBoxItem
{
    public static readonly StyledProperty<MtCommandVariant> VariantProperty = AvaloniaProperty.Register<MtCommandItem, MtCommandVariant>(
        nameof(Variant));

    public MtCommandVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
