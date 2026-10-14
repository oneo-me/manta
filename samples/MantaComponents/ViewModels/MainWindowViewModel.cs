using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Styling;

namespace MantaComponents.ViewModels;

sealed class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        ToggleThemeCommand = new RelayCommand(_ => ToggleTheme());
        IncrementCommand = new RelayCommand(_ => Count++);
        ResetCommand = new RelayCommand(_ => Reset());
        AddItemCommand = new RelayCommand(_ => Items.Add(new($"Item {Items.Count + 1}", "Added at runtime.")));
        RemoveItemCommand = new RelayCommand(_ => RemoveSelectedItem(), _ => SelectedItem is not null);
    }

    public ICommand ToggleThemeCommand { get; }

    public ICommand IncrementCommand { get; }

    public ICommand ResetCommand { get; }

    public ICommand AddItemCommand { get; }

    public RelayCommand RemoveItemCommand { get; }

    string _name = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            if (SetProperty(ref _name, value))
                OnPropertyChanged(nameof(Greeting));
        }
    }

    public string Greeting => string.IsNullOrWhiteSpace(Name) ? "Type a name to see the greeting." : $"Hello, {Name}!";

    int _count;

    public int Count
    {
        get => _count;
        set
        {
            if (SetProperty(ref _count, value))
                OnPropertyChanged(nameof(CounterText));
        }
    }

    public string CounterText => Count switch
    {
        0 => "Not clicked yet.",
        1 => "Clicked once.",
        _ => $"Clicked {Count} times."
    };

    bool _notifications = true;

    public bool Notifications
    {
        get => _notifications;
        set => SetProperty(ref _notifications, value);
    }

    bool _compact;

    public bool Compact
    {
        get => _compact;
        set => SetProperty(ref _compact, value);
    }

    double _volume = 60;

    public double Volume
    {
        get => _volume;
        set => SetProperty(ref _volume, value);
    }

    decimal _quantity = 1;

    public decimal Quantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value);
    }

    public string[] Frameworks { get; } = ["Avalonia", "WPF", "WinUI", "MAUI"];

    string _framework = "Avalonia";

    public string Framework
    {
        get => _framework;
        set => SetProperty(ref _framework, value);
    }

    public ObservableCollection<DemoItem> Items { get; } =
    [
        new("MButton", "Buttons with the Manta variants."),
        new("MTextBox", "Text input styled by the Manta theme."),
        new("MListBox", "List with Manta item containers."),
        new("MProgressBar", "Value display with the Manta palette.")
    ];

    DemoItem? _selectedItem;

    public DemoItem? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (SetProperty(ref _selectedItem, value))
                RemoveItemCommand.RaiseCanExecuteChanged();
        }
    }

    string _theme = "Light";

    public string Theme
    {
        get => _theme;
        private set => SetProperty(ref _theme, value);
    }

    void ToggleTheme()
    {
        var theme = Theme == "Dark" ? "Light" : "Dark";
        Theme = theme;

        if (Application.Current is { } app)
            app.RequestedThemeVariant = theme == "Dark" ? ThemeVariant.Dark : ThemeVariant.Light;
    }

    void Reset()
    {
        Name = string.Empty;
        Count = 0;
        Notifications = true;
        Compact = false;
        Volume = 60;
        Quantity = 1;
        Framework = "Avalonia";
    }

    void RemoveSelectedItem()
    {
        if (SelectedItem is not { } item)
            return;

        var index = Items.IndexOf(item);
        Items.Remove(item);

        if (Items.Count > 0)
            SelectedItem = Items[Math.Min(index, Items.Count - 1)];
    }
}
