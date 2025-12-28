using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public sealed class MtWindow : Window
{
    protected override Type StyleKeyOverride { get; } = typeof(MtWindow);

    public static readonly StyledProperty<bool> AllowFullScreenProperty = AvaloniaProperty.Register<MtWindow, bool>(
        nameof(AllowFullScreen));

    public bool AllowFullScreen
    {
        get => GetValue(AllowFullScreenProperty);
        set => SetValue(AllowFullScreenProperty, value);
    }
}
