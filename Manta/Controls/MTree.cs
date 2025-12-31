using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Manta.Controls;

/// <summary>
///     A tree view control with animated expand/collapse functionality.
/// </summary>
public class MTree : TreeView
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MTreeItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MTreeItem>(item, out recycleKey);
    }

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<MTree, ICommand?>(nameof(Command));

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    internal void OnItemHeaderOnPointerPressed(MTreeItem treeItem)
    {
        if (Command is null)
            return;

        var data = treeItem.DataContext is null || DataContext == treeItem.DataContext
            ? treeItem
            : treeItem.DataContext;

        if (Command.CanExecute(data)) Command.Execute(data);
    }
}

/// <summary>
///     An item in the MTree control that can contain child items and supports animated expand/collapse.
/// </summary>
public class MTreeItem : TreeViewItem
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MTreeItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MTreeItem>(item, out recycleKey);
    }

    Border? PART_Header;
    Border? PART_ExpandCollapseIcon;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Header = e.NameScope.Get<Border>(nameof(PART_Header));
        PART_ExpandCollapseIcon = e.NameScope.Get<Border>(nameof(PART_ExpandCollapseIcon));

        PART_Header.PointerPressed += HeaderOnPointerPressed;
        PART_Header.DoubleTapped += HeaderOnDoubleTapped;
        PART_ExpandCollapseIcon.PointerPressed += ExpandCollapseIconOnPointerPressed;
    }

    void HeaderOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var parent = this.GetLogicalParent<MTree>();
        parent?.OnItemHeaderOnPointerPressed(this);
    }

    void HeaderOnDoubleTapped(object? sender, TappedEventArgs e)
    {
        Switch();
    }

    void ExpandCollapseIconOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        e.Handled = true;
        Switch();
    }

    void Switch()
    {
        if (Items.Count > 0)
            IsExpanded = !IsExpanded;
    }
}
