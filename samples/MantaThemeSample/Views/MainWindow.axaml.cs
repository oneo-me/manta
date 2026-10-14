using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Styling;

namespace MantaThemeSample.Views;

sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    void OnToggleTheme(object? sender, TappedEventArgs e)
    {
        if (Application.Current is not { } app)
            return;

        app.RequestedThemeVariant = app.ActualThemeVariant == ThemeVariant.Dark
            ? ThemeVariant.Light
            : ThemeVariant.Dark;
    }
}
