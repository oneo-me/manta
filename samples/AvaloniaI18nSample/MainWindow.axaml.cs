using Avalonia.Controls;
using Manta.Globalization;
using Manta.Globalization.Core;

namespace AvaloniaI18nSample;

public partial class MainWindow : Window
{
    static readonly LangType[] Languages = [LangType.en, LangType.zh_Hans];

    public MainWindow()
    {
        InitializeComponent();

        LanguageSelector.ItemsSource = Languages;
        LanguageSelector.SelectedItem = Languages.Contains(Lang.Current) ? Lang.Current : LangType.en;
        Lang.Current = (LangType)LanguageSelector.SelectedItem!;
    }

    void OnLanguageSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (LanguageSelector.SelectedItem is LangType langType)
            Lang.Current = langType;
    }
}
