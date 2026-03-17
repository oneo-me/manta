using Avalonia.Controls;
using Manta.Native.Platform.macOS;

namespace Manta.Native;

public static class MantaWindow
{
    public static void ConfigureWindow(Window window)
    {
        if (OperatingSystem.IsMacOS() is false)
            return;

        var windowPtr = GetNativeWindowHandle(window);
        if (windowPtr != IntPtr.Zero)
            MacOSWindow.ConfigureWindow(windowPtr);
    }

    static IntPtr GetNativeWindowHandle(Window window)
    {
        try
        {
            var platformHandle = window.TryGetPlatformHandle();
            return platformHandle?.Handle ?? IntPtr.Zero;
        }
        catch
        {
            return IntPtr.Zero;
        }
    }

    public static async Task ShowSheetAsync(Window owner, Window window, Action<Window> processSheet)
    {
        if (OperatingSystem.IsMacOS())
        {
            var ownerPtr = GetNativeWindowHandle(owner);
            var windowPtr = GetNativeWindowHandle(window);
            if (ownerPtr != IntPtr.Zero && windowPtr != IntPtr.Zero)
            {
                processSheet(owner);
                await MacOSWindow.ShowSheetAsync(ownerPtr, windowPtr);
                return;
            }
        }

        await window.ShowDialog(owner);
    }
}
