using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class LightTabStrip : ListBox
{
    protected override Type StyleKeyOverride { get; } = typeof(LightTabStrip);

    public static readonly StyledProperty<LightTabStripStyle> CustomProperty = AvaloniaProperty.Register<LightTabStrip, LightTabStripStyle>(
        nameof(Custom));

    public LightTabStripStyle Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new LightTabStripItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<LightTabStripItem>(item, out recycleKey);
    }
}
