namespace AirKit.Demo.Configs;

sealed class AppConfig
{
    public MainWindowConfig MainWindow { get; private set; } = new();
    public string? SelectedSample { get; set; }
}
