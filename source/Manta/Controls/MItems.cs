using Avalonia.Controls;

namespace Manta.Controls;

public class MItems : ItemsControl
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MItem>(item, out recycleKey);
    }
}

public class MItem : ContentControl;
