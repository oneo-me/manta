using Avalonia;
using Avalonia.Collections;
using Avalonia.LogicalTree;

namespace AirKit.Reactive.Extensions;

public static class ContentExtensions
{
    public static void ContentChanged(this IAvaloniaList<ILogical> logicalChildren, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ILogical oldValue)
            logicalChildren.Remove(oldValue);

        if (e.NewValue is not ILogical newValue)
            return;

        logicalChildren.Add(newValue);
    }
}
