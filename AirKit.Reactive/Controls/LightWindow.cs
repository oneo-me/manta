using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Media;
using Avalonia.Platform;

namespace AirKit.Reactive.Controls;

public sealed class LightWindow : Window
{
    protected override Type StyleKeyOverride { get; } = typeof(LightWindow);

    public static readonly StyledProperty<IBrush?> BackgroundThemeBrushProperty = AvaloniaProperty.Register<LightWindow, IBrush?>(
        nameof(BackgroundThemeBrush));

    public IBrush? BackgroundThemeBrush
    {
        get => GetValue(BackgroundThemeBrushProperty);
        set => SetValue(BackgroundThemeBrushProperty, value);
    }

    public static readonly StyledProperty<double> BackgroundOpacityProperty = AvaloniaProperty.Register<LightWindow, double>(
        nameof(BackgroundOpacity));

    public double BackgroundOpacity
    {
        get => GetValue(BackgroundOpacityProperty);
        set => SetValue(BackgroundOpacityProperty, value);
    }

    internal LightWindow()
    {
        if (Debugger.IsAttached)
            this.AttachDevTools(new DevToolsOptions());

        var isLinux = OperatingSystem.IsLinux();

        if (isLinux)
            return;

        var isWindows = OperatingSystem.IsWindows();
        var isMacOs = OperatingSystem.IsMacOS();

        if (isWindows)
            PseudoClasses.Add(":windows");

        TransparencyLevelHint =
        [
            WindowTransparencyLevel.AcrylicBlur,
            WindowTransparencyLevel.Blur,
            WindowTransparencyLevel.Mica,
            WindowTransparencyLevel.None
        ];

        ExtendClientAreaToDecorationsHint = true;
        ExtendClientAreaTitleBarHeightHint = 3;

        if (isWindows)
            ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.SystemChrome;
        else if (isMacOs)
            ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.PreferSystemChrome | ExtendClientAreaChromeHints.OSXThickTitleBar;
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        this.FocusActivate();
    }
}
