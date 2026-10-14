using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using ContextMenu = Avalonia.Controls.ContextMenu;
using Control = Avalonia.Controls.Control;
using Menu = Avalonia.Controls.Menu;
using MenuItem = Avalonia.Controls.MenuItem;
using Separator = Avalonia.Controls.Separator;

namespace Manta.Controls;

public class MMenu : Menu
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MMenuItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        var flag = item switch
        {
            MMenuItem or Separator => true,
            _                      => false
        };

        if (flag)
        {
            recycleKey = null;
            return false;
        }

        recycleKey = DefaultRecycleKey;
        return true;
    }
}

public class MMenuItem : MenuItem
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MMenuItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        var flag = item switch
        {
            MMenuItem or Separator => true,
            _                      => false
        };

        if (flag)
        {
            recycleKey = null;
            return false;
        }

        recycleKey = DefaultRecycleKey;
        return true;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        var parentIsMenu = this.FindAncestorOfType<MMenu>() is not null;
        PseudoClasses.Set(":top-level", parentIsMenu);
    }
}

public class MContextMenu : ContextMenu
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MMenuItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        var flag = item switch
        {
            MMenuItem or Separator => true,
            _                      => false
        };

        if (flag)
        {
            recycleKey = null;
            return false;
        }

        recycleKey = DefaultRecycleKey;
        return true;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsOpenProperty)
            PseudoClasses.Set(":open", change.GetNewValue<bool>());
    }
}
