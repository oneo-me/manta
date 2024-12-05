using Avalonia;
using Avalonia.Controls;

namespace AirKit.Controls.Properties;

public static class DesignMode
{
    public static readonly AttachedProperty<bool> IsDesignProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, AvaloniaObject, bool>("IsDesign", Design.IsDesignMode);

    public static bool GetIsDesign(AvaloniaObject obj)
    {
        return obj.GetValue(IsDesignProperty);
    }
}
