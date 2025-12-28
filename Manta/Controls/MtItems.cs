using Avalonia.Controls;

namespace Manta.Controls;

public class MtItems : ItemsControl
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MtItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MtItem>(item, out recycleKey);
    }
}

public class MtItem : ContentControl;
