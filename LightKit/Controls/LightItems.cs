using Avalonia.Controls;

namespace LightKit.Controls;

public class LightItems : ItemsControl
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new LightItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<LightItem>(item, out recycleKey);
    }
}

public class LightItem : ContentControl;
