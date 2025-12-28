using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public sealed class LightWindow : Window
{
    protected override Type StyleKeyOverride { get; } = typeof(LightWindow);

    public static readonly StyledProperty<bool> AllowFullScreenProperty = AvaloniaProperty.Register<LightWindow, bool>(
        nameof(AllowFullScreen));

    public bool AllowFullScreen
    {
        get => GetValue(AllowFullScreenProperty);
        set => SetValue(AllowFullScreenProperty, value);
    }
}
