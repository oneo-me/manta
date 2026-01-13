# Views and ViewModels Guide

This guide covers the MVVM architecture in Manta, including different Model types and their usage.

## Model Hierarchy

```
Model                    # Base class with INotifyPropertyChanged
  └── ViewModel          # For content views (MView)
        ├── WindowModel  # For window views (MWindow)
        └── RegionModel  # For region-based navigation
```

## Model (Base Class)

The base class for all models, implementing `INotifyPropertyChanged`.

```csharp
public abstract class Model : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null);
    protected bool SetValue<T>(ref T backingField, T newValue, [CallerMemberName] string? propertyName = null);
}
```

### Usage

```csharp
public class UserModel : Model
{
    string _name = "";
    public string Name
    {
        get => _name;
        set => SetValue(ref _name, value);  // Auto-notifies PropertyChanged
    }
}
```

## ViewModel

For content views that display within a container.

### Basic Structure

```csharp
using System.Reactive.Disposables;
using Manta.Controls;
using Manta.Reactive.Views;

namespace YourApp.Views;

[View<MyView>]
sealed class MyView_Model : ViewModel
{
    // Design-time instance for XAML previewer
    public static MyView_Model Design => new();

    // Properties
    string _title = "Hello";
    public string Title
    {
        get => _title;
        set => SetValue(ref _title, value);
    }

    // Called once when view is first initialized
    protected override void OnInitialized()
    {
        // One-time setup
    }

    // Called each time view is activated
    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        // Get typed reference to view
        var view = this.GetView<MyView>();

        // Reactive subscriptions (always dispose!)
        someObservable
            .Subscribe(value => { /* handle */ })
            .DisposeWith(disposables);
    }
}
```

### View XAML

```xml
<MView
    x:Class="YourApp.Views.MyView"
    xmlns="https://github.com/avaloniaui"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="clr-namespace:YourApp.Views"
    x:DataType="local:MyView_Model"
    Design.DataContext="{x:Static local:MyView_Model.Design}">

    <TextBlock Text="{Binding Title}" />
</MView>
```

### View Code-Behind

```csharp
using Manta.Controls;

namespace YourApp.Views;

sealed partial class MyView : MView
{
    public MyView()
    {
        InitializeComponent();
    }
}
```

## WindowModel

For window-based views with window lifecycle hooks.

### Basic Structure

```csharp
[View<MainWindow>]
sealed class MainWindow_Model : WindowModel
{
    public static MainWindow_Model Design => new();

    // Configure window properties
    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);

        window.Title = "My Application";
        window.MinWidth = window.Width = 1200;
        window.MinHeight = window.Height = 800;

        // Optional: Center on screen
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);

        var view = this.GetView<MainWindow>();

        // Handle window state changes
        view.GetWindowStatusObservable()
            .Subscribe(status =>
            {
                // status.IsFullScreen
                // status.IsShowButtons
                // status.Offset (title bar insets)
            })
            .DisposeWith(disposables);

        // Bind title bar drag events
        view.TitleBar
            .BindTitleBarEvents()
            .DisposeWith(disposables);
    }
}
```

### Opening Windows

```csharp
using Manta.Reactive.Views;

// Open a new window
var window = ViewManager.OpenWindow<SettingsWindow_Model>();

// Open with context data
var window = ViewManager.OpenWindow<SettingsWindow_Model>(context: new()
{
    ["userId"] = 123,
    ["mode"] = "edit"
});
```

### Accessing Context

```csharp
protected override void OnInitialized()
{
    if (Context?.TryGetValue("userId", out var value) == true && value is int userId)
    {
        // Use userId
    }
}
```

## RegionModel

For views that participate in region-based navigation (e.g., dialogs, panels).

### Basic Structure

```csharp
[View<DialogView>]
sealed class DialogView_Model : RegionModel
{
    public static DialogView_Model Design => new();

    // Close the region view
    public void CloseDialog()
    {
        Close();  // Inherited from RegionModel
    }

    // Called when closed
    protected override void OnClosed()
    {
        // Cleanup
    }
}
```

## ViewManager

Manages navigation within a single view host.

### Usage in ViewModel

```csharp
public class NavigationViewModel : ViewModel
{
    readonly ViewManager _viewManager = new();

    public ViewManager ViewManager => _viewManager;

    public void NavigateToSettings()
    {
        _viewManager.OpenView<SettingsView_Model>();
    }

    public void NavigateToProfile(int userId)
    {
        _viewManager.OpenView<ProfileView_Model>(vm =>
        {
            vm.UserId = userId;
        });
    }

    public void ClearNavigation()
    {
        _viewManager.ClearView();
    }
}
```

### Usage in XAML

```xml
<MViewHost ViewManager="{Binding ViewManager}" />
```

## Common Patterns

### Reactive Subscriptions

Always dispose subscriptions to prevent memory leaks:

```csharp
protected override void OnViewActivated(CompositeDisposable disposables)
{
    // Observable subscription
    _dataService.DataChanged
        .ObserveOn(AvaloniaScheduler.Instance)
        .Subscribe(data => UpdateUI(data))
        .DisposeWith(disposables);

    // Event subscription
    view.Button.Click += OnButtonClick;
    Disposable.Create(() => view.Button.Click -= OnButtonClick)
        .DisposeWith(disposables);
}
```

### Accessing Named Elements

```csharp
protected override void OnViewActivated(CompositeDisposable disposables)
{
    var view = this.GetView<MyView>();

    // Access named elements from XAML
    view.MyButton.IsEnabled = false;
    view.MyTextBox.Focus();
}
```

### Window Status

Handle window state changes (fullscreen, buttons visibility):

```csharp
view.GetWindowStatusObservable()
    .Subscribe(status =>
    {
        // Adjust UI for fullscreen
        view.TitleBarButtons.IsVisible = status is { IsShowButtons: true, IsFullScreen: false };

        // Handle title bar offset (macOS traffic lights)
        view.Sidebar.Margin = status.Offset.Top > 0
            ? new Thickness(0, status.Offset.Top, 0, 0)
            : default;
    })
    .DisposeWith(disposables);
```

## Summary

| Type | Use Case | Container |
|------|----------|-----------|
| `Model` | Data models, non-UI | N/A |
| `ViewModel` | Content views | `MView` + `MViewHost` |
| `WindowModel` | Windows | `MWindow` |
| `RegionModel` | Dialogs, panels | Region system |

## Common Imports

```csharp
using System.Reactive.Disposables;
using Avalonia.Controls;
using Manta.Controls;
using Manta.Extensions;
using Manta.Reactive.Extensions;
using Manta.Reactive.Views;
```
