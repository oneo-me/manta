using System.Diagnostics.CodeAnalysis;
using Avalonia;

namespace Axe.Reactive.Controls;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class CurrentSystem
{
    public static readonly AttachedProperty<bool> IsWindowsProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, AvaloniaObject, bool>("IsWindows", OperatingSystem.IsWindows());

    public static bool GetIsWindows(AvaloniaObject obj)
    {
        return obj.GetValue(IsWindowsProperty);
    }

    public static readonly AttachedProperty<bool> IsLinuxProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, AvaloniaObject, bool>("IsLinux", OperatingSystem.IsLinux());

    public static bool GetIsLinux(AvaloniaObject obj)
    {
        return obj.GetValue(IsLinuxProperty);
    }

    public static readonly AttachedProperty<bool> IsMacOSProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, AvaloniaObject, bool>("IsMacOS", OperatingSystem.IsMacOS());

    public static bool GetIsMacOS(AvaloniaObject obj)
    {
        return obj.GetValue(IsMacOSProperty);
    }
}
