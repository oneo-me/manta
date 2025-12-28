using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public class LightCommand : ListBox
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new LightCommandItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<LightCommandItem>(item, out recycleKey);
    }

    public static readonly StyledProperty<string> WatermarkProperty = AvaloniaProperty.Register<LightCommand, string>(
        nameof(Watermark));

    public string Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<LightCommand, string>(
        nameof(Text));

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}

public enum LightCommandVariant
{
    Default,
    Separator,
    Group,
    NoResults
}

public class LightCommandItem : ListBoxItem
{
    public static readonly StyledProperty<LightCommandVariant> VariantProperty = AvaloniaProperty.Register<LightCommandItem, LightCommandVariant>(
        nameof(Variant));

    public LightCommandVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
