using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;

namespace LightKit.Avalonia;

public static class CurrentSystem
{
    public static readonly AttachedProperty<bool> IsWindowsProperty =
        AvaloniaProperty.RegisterAttached<Control, Control, bool>("IsWindows", OperatingSystem.IsWindows());

    public static bool GetIsWindows(Control obj)
    {
        return obj.GetValue(IsWindowsProperty);
    }

    public static readonly AttachedProperty<bool> IsLinuxProperty =
        AvaloniaProperty.RegisterAttached<Control, Control, bool>("IsLinux", OperatingSystem.IsLinux());

    public static bool GetIsLinux(Control obj)
    {
        return obj.GetValue(IsLinuxProperty);
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static readonly AttachedProperty<bool> IsMacOSProperty =
        AvaloniaProperty.RegisterAttached<Control, Control, bool>("IsMacOS", OperatingSystem.IsMacOS());

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static bool GetIsMacOS(Control obj)
    {
        return obj.GetValue(IsMacOSProperty);
    }
}
