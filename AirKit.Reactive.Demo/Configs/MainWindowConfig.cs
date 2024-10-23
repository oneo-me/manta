using Avalonia.Controls;

namespace AirKit.Reactive.Demo.Configs;

public class MainWindowConfig
{
    public WindowState WindowState { get; set; } = WindowState.Normal;
    public int Left { get; set; }
    public int Top { get; set; }
    public double Width { get; set; } = 820;
    public double Height { get; set; } = 660;
}
