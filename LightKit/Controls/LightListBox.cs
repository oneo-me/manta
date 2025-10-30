using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls;

public class LightListBox : ListBox
{
    public static readonly StyledProperty<LightListBoxStyle> CustomProperty = AvaloniaProperty.Register<LightListBox, LightListBoxStyle>(
        nameof(Custom));

    public LightListBoxStyle Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
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
