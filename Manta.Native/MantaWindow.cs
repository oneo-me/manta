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
}
