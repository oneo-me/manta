using Avalonia;
using Avalonia.Controls;

namespace Axe.Reactive.Controls;

public static class DesignMode
{
    public static readonly AttachedProperty<bool> IsDesignProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, AvaloniaObject, bool>("IsDesign", Design.IsDesignMode);

    public static bool GetIsDesign(AvaloniaObject obj)
    {
        return obj.GetValue(IsDesignProperty);
    }
}
