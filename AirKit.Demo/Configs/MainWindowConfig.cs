using Avalonia.Controls;

namespace AirKit.Demo.Configs;

sealed class MainWindowConfig
{
    public WindowState WindowState { get; set; } = WindowState.Normal;
    public int Left { get; set; }
    public int Top { get; set; }
    public double Width { get; set; } = 820;
    public double Height { get; set; } = 660;
}
