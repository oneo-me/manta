using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace AirKit.Reactive.Controls;

public sealed class LightWindow : Window
{
    protected override Type StyleKeyOverride { get; } = typeof(LightWindow);

    public new static readonly StyledProperty<Bitmap?> IconProperty = AvaloniaProperty.Register<LightWindow, Bitmap?>(
        nameof(Icon));

    public new Bitmap? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IBrush?> BackgroundThemeBrushProperty = AvaloniaProperty.Register<LightWindow, IBrush?>(
        nameof(BackgroundThemeBrush));

    public IBrush? BackgroundThemeBrush
    {
        get => GetValue(BackgroundThemeBrushProperty);
        set => SetValue(BackgroundThemeBrushProperty, value);
    }

    internal LightWindow()
    {
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

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IconProperty)
            if (change.NewValue is Bitmap bitmap)
                base.Icon = new(bitmap);
            else
                base.Icon = null;
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        this.FocusActivate();
    }
}
