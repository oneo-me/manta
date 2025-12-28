using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public class LightTabStrip : ListBox
{
    public static readonly StyledProperty<LightTabStripType> TypeProperty = AvaloniaProperty.Register<LightTabStrip, LightTabStripType>(
        nameof(Type));

    public LightTabStripType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
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
