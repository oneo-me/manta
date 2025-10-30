using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class LightListBox : ListBox
{
    public static readonly StyledProperty<LightListBoxType> TypeProperty = AvaloniaProperty.Register<LightListBox, LightListBoxType>(
        nameof(Type));

    public LightListBoxType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new LightListBoxItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<LightListBoxItem>(item, out recycleKey);
    }
}
