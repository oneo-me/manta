using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public sealed class MWindow : Window
{
    protected override Type StyleKeyOverride { get; } = typeof(MWindow);

    public static readonly StyledProperty<bool> AllowFullScreenProperty = AvaloniaProperty.Register<MWindow, bool>(
        nameof(AllowFullScreen));

    public bool AllowFullScreen
    {
        get => GetValue(AllowFullScreenProperty);
        set => SetValue(AllowFullScreenProperty, value);
    }
}
