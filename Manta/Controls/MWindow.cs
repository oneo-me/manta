using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

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

    public MWindow()
    {
        if (OperatingSystem.IsMacOS() is false)
            return;

        KeyUp += OnKeyUp;
    }

    void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.W && e.KeyModifiers.HasFlag(KeyModifiers.Meta))
            Close();
    }
}
