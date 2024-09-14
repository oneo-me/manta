using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Platform;

namespace AirKit.Reactive.Controls;

public sealed class LightWindow : Window
{
    protected override Type StyleKeyOverride { get; } = typeof(LightWindow);

    internal LightWindow()
    {
        if (Debugger.IsAttached)
            this.AttachDevTools(new DevToolsOptions());

        if (OperatingSystem.IsWindows())
            PseudoClasses.Add(":windows");

        if (OperatingSystem.IsLinux() is false)
        {
            TransparencyLevelHint =
            [
                WindowTransparencyLevel.AcrylicBlur,
                WindowTransparencyLevel.Blur,
                WindowTransparencyLevel.Mica,
                WindowTransparencyLevel.None
            ];

            ExtendClientAreaToDecorationsHint = true;
            if (OperatingSystem.IsWindows())
            {
                ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.SystemChrome;
                ExtendClientAreaTitleBarHeightHint = 3;
            }
            else if (OperatingSystem.IsMacOS())
            {
                ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.PreferSystemChrome | ExtendClientAreaChromeHints.OSXThickTitleBar;
            }
        }
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        this.FocusActivate();
    }
}
