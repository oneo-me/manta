using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public class MFlyout : Flyout
{
    protected override Control CreatePresenter()
    {
        return new MFlyoutPresenter
        {
            [!ContentControl.ContentProperty] = this[!ContentProperty],
            [!ContentControl.ContentTemplateProperty] = this[!ContentTemplateProperty]
        };
    }

    protected override void OnOpening(CancelEventArgs args)
    {
        base.OnOpening(args);

        if (Popup.Child is MFlyoutPresenter flyoutPresenter)
            flyoutPresenter.Placement = Placement;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (Popup.Child is not MFlyoutPresenter flyoutPresenter)
            return;

        if (change.Property == IsOpenProperty)
            flyoutPresenter.OnIsOpenChanged(change.GetNewValue<bool>());
    }
}

public class MFlyoutPresenter : FlyoutPresenter
{
    public static readonly StyledProperty<PlacementMode> PlacementProperty = AvaloniaProperty.Register<MFlyoutPresenter, PlacementMode>(
        nameof(Placement), PlacementMode.Bottom);

    public PlacementMode Placement
    {
        get => GetValue(PlacementProperty);
        internal set => SetValue(PlacementProperty, value);
    }

    public void OnIsOpenChanged(bool isOpen)
    {
        PseudoClasses.Set(":open", isOpen);
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

    protected override void OnOpening(CancelEventArgs args)
    {
        base.OnOpening(args);

        if (Popup.Child is MMenuFlyoutPresenter menuFlyoutPresenter)
            menuFlyoutPresenter.Placement = Placement;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (Popup.Child is not MMenuFlyoutPresenter menuFlyoutPresenter)
            return;

        if (change.Property == IsOpenProperty)
            menuFlyoutPresenter.OnIsOpenChanged(change.GetNewValue<bool>());
    }
}

public class MMenuFlyoutPresenter : MenuFlyoutPresenter
{
    public static readonly StyledProperty<PlacementMode> PlacementProperty = AvaloniaProperty.Register<MMenuFlyoutPresenter, PlacementMode>(
        nameof(Placement), PlacementMode.Bottom);

    public PlacementMode Placement
    {
        get => GetValue(PlacementProperty);
        internal set => SetValue(PlacementProperty, value);
    }

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
