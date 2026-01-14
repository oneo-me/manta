using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using ContextMenu = Avalonia.Controls.ContextMenu;
using Control = Avalonia.Controls.Control;
using ItemsControl = Avalonia.Controls.ItemsControl;
using Menu = Avalonia.Controls.Menu;
using MenuFlyout = Avalonia.Controls.MenuFlyout;
using MenuFlyoutPresenter = Avalonia.Controls.MenuFlyoutPresenter;
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

public class MMenuFlyout : MenuFlyout
{
    protected override Control CreatePresenter()
    {
        return new MMenuFlyoutPresenter
        {
            ItemsSource = Items,
            [!ItemsControl.ItemTemplateProperty] = this[!ItemTemplateProperty],
            [!ItemsControl.ItemContainerThemeProperty] = this[!ItemContainerThemeProperty]
        };
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsOpenProperty)
            if (Popup.Child is MMenuFlyoutPresenter menuFlyoutPresenter)
                menuFlyoutPresenter.OnIsOpenChanged(change.GetNewValue<bool>());
    }
}

public class MMenuFlyoutPresenter : MenuFlyoutPresenter
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

    public void OnIsOpenChanged(bool isOpen)
    {
        PseudoClasses.Set(":open", isOpen);
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
