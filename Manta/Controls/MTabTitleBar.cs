using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;

namespace Manta.Controls;

[TemplatePart("PART_RightContentPresenter", typeof(ContentPresenter))]
public class MTabTitleBar : ListBox
{
    static MTabTitleBar()
    {
        RightContentProperty.Changed.AddClassHandler<MTabTitleBar>((x, e) => x.RightContentChanged(e));
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MTabTitleBarItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<MTabTitleBarItem>(item, out recycleKey);
    }

    public static readonly StyledProperty<object?> RightContentProperty = AvaloniaProperty.Register<MTabTitleBar, object?>(
        nameof(RightContent));

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> RightContentTemplateProperty = AvaloniaProperty.Register<MTabTitleBar, IDataTemplate?>(
        nameof(RightContentTemplate));

    public IDataTemplate? RightContentTemplate
    {
        get => GetValue(RightContentTemplateProperty);
        set => SetValue(RightContentTemplateProperty, value);
    }

    void RightContentChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ILogical oldValue)
            LogicalChildren.Remove(oldValue);

        if (e.NewValue is not ILogical newValue)
            return;

        LogicalChildren.Add(newValue);
    }
}

public class MTabTitleBarItem : ListBoxItem;
