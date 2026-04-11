# MVVM Patterns in Manta

## Class Hierarchy

```
Model (INotifyPropertyChanged)
  └── CommandModel (command support)
      └── ViewModel (MView lifecycle)
          ├── RegionModel (tab/region management)
          └── WindowModel (window lifecycle)
              ├── DialogModel (modal dialog, has Result + built-in cancel/continue)
              └── TrayWindowModel (system tray window)
```

> **All views are `MView`** — including main windows and dialogs. There is no separate "window view"
> class. The framework wraps the view in a native window automatically.

## Model — Property Notification

```csharp
// Using a backing field:
private string _name = string.Empty;
public string Name
{
    get => _name;
    set => SetValue(ref _name, value);
}

// Using C# 13 field keyword (preferred):
public string Title
{
    get;
    set => SetValue(ref field, value);
} = "Default";
```

## CommandModel — Defining Commands

`GetCommand()` creates and caches a command by property name (call multiple times = same instance):

```csharp
// Sync
public CommandWarper OpenCommand => GetCommand(() => { /* ... */ });

// Sync with typed parameter
public CommandWarper<string> NavigateCommand => GetCommand<string>(path => { /* ... */ });

// Async
public CommandWarper LoadCommand => GetCommand(async () =>
{
    await Task.Delay(100);
});

// With CanExecute
public CommandWarper SaveCommand => GetCommand(
    () => { /* save */ },
    canAction: () => IsModified
);
```

**`CommandWarper` extra API:**

- `IsExecuting` — `bool`, reactive property; `true` while an async command is running. Bind to a
  loading indicator, or use as CanExecute guard.
- `OnChanged()` — call manually when external state that affects `canAction` has changed; triggers
  `CanExecuteChanged` so the button re-queries `CanExecute`.

```csharp
// Show a spinner while loading
public CommandWarper LoadCommand => GetCommand(async () => await LoadDataAsync());
// In AXAML: IsVisible="{Binding LoadCommand.IsExecuting}"

// Invalidate CanExecute when an external property changes
public string Filter
{
    get;
    set
    {
        SetValue(ref field, value);
        SearchCommand.OnChanged();   // button enabled/disabled re-evaluated
    }
} = string.Empty;
public CommandWarper SearchCommand => GetCommand(() => { }, () => !string.IsNullOrEmpty(Filter));
```

## ViewModel — View Lifecycle

Use for views that are embedded inside a window (regions, panels, sub-views).

```csharp
[View<MyView>]
public sealed class MyView_Model : ViewModel
{
    public static MyView_Model Design => new();  // design-time data context

    // Called once when view first attaches
    protected override void OnInitialized() { }

    // Called each time view attaches to the visual tree
    // All subscriptions added here are automatically disposed when view detaches
    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        MApp.SubscribeThemeChanged(variant => UpdateIcon(variant))
            .DisposeWith(disposables);
    }
}
```

## WindowModel — Top-Level Window

Use for the **main window** and any **standalone windows** that have no parent, or that are opened
as independent application windows.

`OnWindowInitialized(MWindow window)` is called once by the framework after the window is created.
Use it to configure window properties. `MWindow` is an internal Manta type — do not reference it
outside of this override.

```csharp
[View<MainView>]
public sealed class MainView_Model : WindowModel
{
    public static MainView_Model Design => new();

    readonly ConfigService _config = Service.GetService<ConfigService>();

    public override void OnWindowInitialized(MWindow window)
    {
        base.OnWindowInitialized(window);
        window.Title = "My App";
        window.MinWidth = 800;
        window.MinHeight = 600;
        window.Width = _config.Width;
        window.Height = _config.Height;

        window.Closing += delegate
        {
            _config.Width = window.Width;
            _config.Height = window.Height;
        };
    }

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);
        MApp.SubscribeThemeChanged(v => ThemeIcon = GetIcon(v))
            .DisposeWith(disposables);
    }

    public IconSource ThemeIcon { get; set => SetValue(ref field, value); }
    public CommandWarper SwitchThemeCommand => GetCommand(() => MApp.SwitchTheme());
}
```

**Opening a standalone window from another ViewModel:**

```csharp
public CommandWarper OpenSettingsCommand => GetCommand(async () =>
{
    await new SettingsView_Model().ShowDialogAsync(this);
});
```

## DialogModel — Modal Dialog

Use for views that **require a response before the user can continue** — confirmations, alerts,
forms requiring explicit OK/Cancel. DialogModel is a `WindowModel` subclass that pre-configures
the window as a non-resizable, centered modal dialog.

**Built-in API:**
- `ContinueAsyncCommand` — sets `Result = true`, closes the dialog (calls `CanContinueAsync()` override)
- `CancelAsyncCommand` — sets `Result = false`, closes the dialog (calls `CanCancelAsync()` override)
- `Result` — bool result after close

```csharp
[View<ConfirmView>]
public sealed class ConfirmView_Model : DialogModel
{
    public static ConfirmView_Model Design => new();

    public string Message { get; set => SetValue(ref field, value); } = string.Empty;

    // Optional validation before closing
    protected override Task<bool> CanContinueAsync()
    {
        // Return false to prevent closing
        return Task.FromResult(!string.IsNullOrWhiteSpace(InputText));
    }
}
```

AXAML for a dialog — two **mandatory** rules:

1. **Wrap content in `MBackground`** — provides the correct environment background for the dialog window.
2. **Use `MDialogTemplate` as the content container** — provides unified header, cancel, and continue button layout.

```xml
<MView x:Class="MyApp.Views.ConfirmView"
       xmlns="https://github.com/avaloniaui"
       xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
       xmlns:local="clr-namespace:MyApp.Views"
       x:DataType="local:ConfirmView_Model"
       Design.DataContext="{x:Static local:ConfirmView_Model.Design}">
  <MBackground MinWidth="{MSize S80}">
    <MDialogTemplate Header="Confirm"
                     CancelButton="Cancel"
                     ContinueButton="OK"
                     CancelButtonCommand="{Binding CancelAsyncCommand}"
                     ContinueButtonCommand="{Binding ContinueAsyncCommand}">
      <!-- Dialog body goes here as child content -->
      <TextBlock Text="{Binding Message}" TextWrapping="Wrap" />
    </MDialogTemplate>
  </MBackground>
</MView>
```

`MDialogTemplate` properties:

| Property | Type | Description |
|----------|------|-------------|
| `Header` | `string` | Dialog title shown at the top |
| `CancelButton` | `string` | Label for the cancel button |
| `CancelButtonVariant` | `MButtonVariant` | Visual variant for cancel button |
| `CancelButtonCommand` | `ICommand` | Bind to `CancelAsyncCommand` |
| `ContinueButton` | `string` | Label for the confirm/OK button |
| `ContinueButtonVariant` | `MButtonVariant` | Visual variant for continue button |
| `ContinueButtonCommand` | `ICommand` | Bind to `ContinueAsyncCommand` |
| Content | child element | Dialog body content |

> **Do not** place bare `MPanel` or custom button rows directly inside a dialog `MView`. Always use
> `MBackground > MDialogTemplate` — this ensures consistent sizing, background, and button placement
> across all dialogs in the app.

**Showing a dialog and reading the result:**

```csharp
public CommandWarper DeleteCommand => GetCommand(async () =>
{
    var dialog = new ConfirmView_Model { Message = "Delete this item?" };
    await dialog.ShowDialogAsync(this);  // 'this' = owner ViewModel

    if (dialog.Result)
        await DeleteItemAsync();
});
```

## TrayWindowModel — System Tray Popup

`TrayWindowModel` is a `WindowModel` subclass pre-configured as a borderless popup that
**auto-closes when the window loses focus** (`CloseWhenDeactivated = true`).

```csharp
[View<TrayPopupView>]
public sealed class TrayPopupView_Model : TrayWindowModel
{
    public static TrayPopupView_Model Design => new();
    // No window customization needed — defaults already set by framework
}
```

Show it from a tray icon handler:

```csharp
await new TrayPopupView_Model().ShowAsync();
```

**Decision: WindowModel vs DialogModel**

| Scenario | Use |
|----------|-----|
| Main application window | `WindowModel` (via `MAppOptions.MainModel`) |
| Standalone window opened from menu/command | `WindowModel` + `ShowDialogAsync` |
| Confirmation / alert needing OK/Cancel | `DialogModel` |
| Form requiring explicit submit | `DialogModel` |
| System tray popup | `TrayWindowModel` |

## @ViewAttribute — View Binding

Connects ViewModel to View. The view class must be `MView` (or subclass):

```csharp
[View<MyView>]   // matches the class name of the MView subclass
public sealed class MyView_Model : ViewModel { }
```

View AXAML — always use `x:DataType` for compiled bindings:

```xml
<MView x:Class="MyApp.Views.MyView"
    xmlns="https://github.com/avaloniaui"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="clr-namespace:MyApp.Views"
    x:DataType="local:MyView_Model"
    Design.DataContext="{x:Static local:MyView_Model.Design}">
    <!-- content -->
</MView>
```

View code-behind — **only** `InitializeComponent()`:

```csharp
public sealed partial class MyView : MView
{
    public MyView() => InitializeComponent();
}
```

## ViewContext — Parent-to-Child Data Sharing

`GetContext<T>(key)` and `SetContext<T>(key, value)` let a parent ViewModel pass data down to
child ViewModels through the visual tree — without manual property threading.

**Parent provides context:**

```csharp
[View<ParentView>]
public sealed class ParentView_Model : ViewModel
{
    public User CurrentUser { get; } = new User("Alice");

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        base.OnViewActivated(disposables);
        SetContext("current-user", CurrentUser);   // key is arbitrary string
    }
}
```

**Child consumes context:**

```csharp
[View<ChildView>]
public sealed class ChildView_Model : ViewModel
{
    // GetContext walks up the visual tree and returns a reactive holder
    ViewContext<User> _userCtx => GetContext<User>("current-user");

    public string UserName => _userCtx.Value?.Name ?? string.Empty;
}
```

`ViewContext<T>` is a `Model` subclass — bind to `.Value` in AXAML:

```xml
<!-- In ChildView.axaml, if exposing the context as a property -->
<TextBlock Text="{Binding UserName}" />
```

- Context lookup traverses parent `MView` controls upward.
- Resolution happens at view activation time (lazy).
- The `Source` property on the context records which ViewModel provided it.

---

## ViewManager — Single-Slot View Switcher

`ViewManager` manages **one active ViewModel at a time** — simpler than `RegionManager` when you
just need to swap a content area between different views without tabs.

```csharp
public sealed class ShellView_Model : WindowModel
{
    public ViewManager Content { get; } = new();

    public CommandWarper ShowHomeCommand => GetCommand(() =>
        Content.OpenView(() => new HomeView_Model())
    );

    public CommandWarper ShowSettingsCommand => GetCommand(() =>
        Content.OpenView<SettingsView_Model>()   // created via Service.CreateInstance
    );
}
```

AXAML — bind `MViewHost.ViewModel` to `ViewManager.Current`:

```xml
<MViewHost ViewModel="{Binding Content.Current}" />
```

**ViewManager API:**

| Method | Description |
|--------|-------------|
| `OpenView<T>(Func<T> create)` | Open with factory — control creation yourself |
| `OpenView<T>(Action<T>? configure)` | Open via service locator, optional post-config |
| `OpenView<T>(Type type, Action<T>? configure)` | Open subclass typed as T |
| `ClearView()` | Clear and dispose current view |
| `Current` | Currently active ViewModel (bindable) |

---

## ViewTemplate — IDataTemplate for Dynamic Views

Use `ViewTemplate<T>` when you need an `IDataTemplate` that creates views from data objects
(e.g., in `ItemsControl`, `ContentControl`, or `TransitioningContentControl`).

```csharp
// In ViewModel:
public IDataTemplate ItemTemplate { get; } =
    new ViewTemplate<Document>(doc => new DocumentView_Model { Document = doc });
```

```xml
<!-- In AXAML: -->
<ItemsControl ItemsSource="{Binding Documents}"
              ItemTemplate="{Binding ItemTemplate}" />
```

Non-generic form (matches any data):

```csharp
new ViewTemplate(() => new MyView_Model())   // always same ViewModel type
```

---

## RegionModel — Tab / Region ViewModel

Use for views that are managed as tabs or swappable panels within a `RegionManager`. Inherits
`ViewModel`, adds a `Close()` method that notifies the manager.

```csharp
// Abstract base for your app's region type
public abstract class MyRegionModel : RegionModel
{
    public abstract string Header { get; }      // tab label
    public abstract IconSource Icon { get; }    // tab icon
}

// A concrete region
[View<HomeView>]
public sealed class HomeView_Model : MyRegionModel
{
    public static HomeView_Model Design => new();

    public override string Header => "Home";
    public override IconSource Icon => Icons.Home;

    // Called by RegionManager when this region is closed
    protected override void OnClosed()
    {
        // cleanup if needed
    }
}
```

## RegionManager — Tab Navigation

`RegionManager<T>` manages a list of `RegionModel` instances with observable selection.
Bind `Items` to a tab strip and `SelectedItem` to a view host.

```csharp
// Typically created and owned by a service
public sealed class ViewService
{
    public RegionManager<MyRegionModel> RegionManager { get; } = new(
        new HomeView_Model()    // initial regions
    );
}

// Expose via WindowModel
public sealed class MainView_Model : WindowModel
{
    readonly ViewService _viewService = Service.GetService<ViewService>();

    public RegionManager<MyRegionModel> RegionManager => _viewService.RegionManager;

    // Open a region (deduplicates by type)
    public CommandWarper OpenHomeCommand => GetCommand(() =>
        RegionManager.Open(() => new HomeView_Model())
    );

    // Open with context (deduplicates by type + context)
    public CommandWarper<string> OpenFileCommand => GetCommand<string>(path =>
        RegionManager.Open(path, p => new FileView_Model(p))
    );
}
```

AXAML wiring:

```xml
<Grid RowDefinitions="Auto *">
    <MTabTitleBar Grid.Row="0"
                  ItemsSource="{Binding RegionManager.Items}"
                  SelectedItem="{Binding RegionManager.SelectedItem}">
        <MTabTitleBar.ItemTemplate>
            <DataTemplate>
                <TextBlock Text="{Binding Header}" />
            </DataTemplate>
        </MTabTitleBar.ItemTemplate>
    </MTabTitleBar>

    <MViewHost Grid.Row="1"
               ViewModel="{Binding RegionManager.SelectedItem}" />
</Grid>
```

**RegionManager API:**

| Method | Description |
|--------|-------------|
| `Open<T>(Func<T> create, isActive?)` | Open or activate a region by type |
| `Open<T, TCtx>(ctx, Func<TCtx,T> create, isActive?)` | Open with context, deduplicates by type + context |
| `SelectedItem` | Currently active region (two-way bindable) |
| `Items` | Observable collection of open regions |
| `Dispose()` | Close all regions |

**Deduplication with `IRegionTarget<T>`:**

Implement `IRegionTarget<TContext>` on your `RegionModel` to prevent duplicate tabs for the same
data. `Open(..., context, ...)` calls `IsTarget(context)` on existing items before creating a new
one.

```csharp
[View<FileEditorView>]
public sealed class FileEditorView_Model : MyRegionModel, IRegionTarget<string>
{
    public string FilePath { get; init; } = string.Empty;

    // Return true = reuse this tab; false = create a new one
    public bool IsTarget(string context) => FilePath == context;
}

// Usage — won't open a second tab for the same path:
RegionManager.Open<FileEditorView_Model, string>(
    path,
    p => new FileEditorView_Model { FilePath = p }
);
```

## Service — Service Locator

```csharp
// Registration (in App.OnInitialize, before options)
var myService = Service.Register(new MyService());
Service.Register<IMyInterface>(new MyImplementation());

// Consumption (field initializer in ViewModel)
readonly MyService _myService = Service.GetService<MyService>();
```

## Reactive Extensions Utilities

### `ObservableEvent` — Event to Observable

Converts any .NET event to an `IObservable` for Rx composition:

```csharp
var clicks = ObservableEvent.FromHandler<RoutedEventArgs>(
    h => button.Click += h,
    h => button.Click -= h
);

clicks
    .Select(e => e.args)
    .Throttle(TimeSpan.FromMilliseconds(300))
    .ObserveOnDispatcher()          // marshal to UI thread
    .Subscribe(args => DoSearch())
    .DisposeWith(disposables);
```

### `ObserveOnDispatcher()` — Marshal to UI Thread

Extension on `IObservable<T>` that routes emissions through the Avalonia dispatcher:

```csharp
someBackgroundObservable
    .ObserveOnDispatcher()
    .Subscribe(value => Items.Add(value));   // safe to mutate UI collections
```

### `ReactiveValue<T>` — Standalone Reactive Property

A lightweight `Model` wrapper for a single value with `INotifyPropertyChanged`:

```csharp
var counter = new ReactiveValue<int>(0);
counter.Value++;                     // fires PropertyChanged
// Bind in AXAML: {Binding MyCounter.Value}
```

---

## App.axaml.cs — Wiring Everything Together

```csharp
sealed class App : MApp
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    protected override void OnInitialize(out MAppOptions options)
    {
        var configService = Service.Register(new ConfigService());
        Service.Register(new ViewService());

        options = new()
        {
            MainModel = () => new MainView_Model(),
            Exit = () => configService.Save()
        };
    }
}
```
