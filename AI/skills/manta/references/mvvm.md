# MVVM Framework

## Class Hierarchy

```
Model (INotifyPropertyChanged)
  └── CommandModel (command caching)
        └── ViewModel (MView lifecycle, ViewContext)
              ├── RegionModel (tab / region management)
              └── WindowModel (window lifecycle)
                    └── DialogModel (modal dialog with Result + Cancel/Continue)
```

> **All views are `MView`** — including main windows and dialogs. There is no separate
> window AXAML class. The framework wraps the view in a native window automatically.

---

## Model — Property Notification

```csharp
// C# 13 field keyword (preferred)
public string Title
{
    get;
    set => SetValue(ref field, value);
} = "Default";

// Explicit backing field
private string _name = string.Empty;
public string Name
{
    get => _name;
    set => SetValue(ref _name, value);
}
```

---

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

### CommandWarper Extra API

| Member | Type | Description |
|--------|------|-------------|
| `IsExecuting` | `bool` (reactive) | `true` while an async command is running. Bind to loading indicators |
| `OnChanged()` | `void` | Manually trigger `CanExecuteChanged` when external state changes |

```csharp
// Show spinner while loading
// AXAML: IsVisible="{Binding LoadCommand.IsExecuting}"
public CommandWarper LoadCommand => GetCommand(async () => await LoadDataAsync());

// Invalidate CanExecute when a property changes
public string Filter
{
    get;
    set
    {
        SetValue(ref field, value);
        SearchCommand.OnChanged();   // re-evaluates CanExecute
    }
} = string.Empty;

public CommandWarper SearchCommand => GetCommand(
    () => { /* search */ },
    () => !string.IsNullOrEmpty(Filter)
);
```

---

## ViewModel — View Lifecycle

Use for views embedded inside a window (regions, panels, sub-views).

```csharp
[View<MyView>]
public sealed partial class MyView_Model : ViewModel
{
    // Called once when view first attaches
    protected override void OnInitialized() { }

    // Called each time view attaches to the visual tree.
    // All subscriptions are auto-disposed when view detaches.
    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        MApp.SubscribeThemeChanged(variant => UpdateIcon(variant))
            .DisposeWith(disposables);
    }
}
```

---

## WindowModel — Top-Level Window

Use for the **main window** and any **standalone windows**.

`OnWindowInitialized(MWindow window)` is called once after the window is created.
Use it to configure window properties.

```csharp
[View<MainView>]
public sealed partial class MainView_Model : WindowModel
{
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

---

## DialogModel — Modal Dialog

Use for views that **require a response** before the user can continue — confirmations, alerts,
forms that need explicit OK/Cancel. Subclass of `WindowModel` that pre-configures the window as a
non-resizable, centered modal.

**Built-in API:**

| Member | Description |
|--------|-------------|
| `ContinueAsyncCommand` | Sets `Result = true`, calls `CanContinueAsync()`, closes dialog |
| `CancelAsyncCommand` | Sets `Result = false`, calls `CanCancelAsync()`, closes dialog |
| `Result` | `bool` — the dialog outcome |

```csharp
[View<ConfirmView>]
public sealed partial class ConfirmView_Model : DialogModel
{
    public string Message { get; set => SetValue(ref field, value); } = string.Empty;

    // Optional: prevent closing if input is invalid
    protected override Task<bool> CanContinueAsync()
    {
        return Task.FromResult(!string.IsNullOrWhiteSpace(InputText));
    }
}
```

**Dialog View AXAML — two mandatory rules:**

1. **Wrap content in `MBackground`** — provides the correct environment background.
2. **Use `MDialogTemplate` as the content container** — provides unified header, cancel, and
   continue button layout.

```xml
<MView x:Class="MyApp.Views.ConfirmView"
       xmlns="https://github.com/avaloniaui"
       xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
       xmlns:local="clr-namespace:MyApp.Views"
       x:DataType="local:ConfirmView_Model">
  <Design.DataContext>
    <local:ConfirmView_Model />
  </Design.DataContext>
  <MBackground MinWidth="{MSize S80}">
    <MDialogTemplate Header="Confirm"
                     CancelButton="Cancel"
                     ContinueButton="OK"
                     CancelButtonCommand="{Binding CancelAsyncCommand}"
                     ContinueButtonCommand="{Binding ContinueAsyncCommand}">
      <TextBlock Text="{Binding Message}" TextWrapping="Wrap" />
    </MDialogTemplate>
  </MBackground>
</MView>
```

**MDialogTemplate properties:**

| Property | Type | Description |
|----------|------|-------------|
| `Header` | `string` | Dialog title at the top |
| `CancelButton` | `string` | Cancel button label |
| `CancelButtonVariant` | `MButtonVariant` | Cancel button visual style |
| `CancelButtonCommand` | `ICommand` | Bind to `CancelAsyncCommand` |
| `ContinueButton` | `string` | Confirm/OK button label |
| `ContinueButtonVariant` | `MButtonVariant` | Confirm button visual style |
| `ContinueButtonCommand` | `ICommand` | Bind to `ContinueAsyncCommand` |
| Content | child element | Dialog body content |

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

---

## Quick Dialogs — MAlert & MConfirm

For simple cases that don't need a custom dialog view:

```csharp
// Alert (informational, one button)
await MAlert.ShowDialogAsync(this, new("Alert Title", "This is the message."));

// Alert with custom button
await MAlert.ShowDialogAsync(this, new("Done", "Export complete.")
{
    ContinueButton = "OK",
    ContinueButtonVariant = MButtonVariant.Default
});

// Confirm (two buttons, returns bool)
var confirmed = await MConfirm.ShowDialogAsync(this, new("Delete?", "This cannot be undone.")
{
    CancelButton = "Cancel",
    ContinueButton = "Delete",
    ContinueButtonVariant = MButtonVariant.Destructive
});

if (confirmed)
    await DeleteAsync();
```

**AlertOptions:**

| Property | Default |
|----------|---------|
| `Title` | (required) |
| `Content` | (required) |
| `ContinueButton` | Localized "Continue" |
| `ContinueButtonVariant` | `MButtonVariant.Default` |

**ConfirmOptions:**

| Property | Default |
|----------|---------|
| `Title` | (required) |
| `Content` | (required) |
| `CancelButton` | Localized "Cancel" |
| `ContinueButton` | Localized "Continue" |
| `CancelButtonVariant` | `MButtonVariant.Outline` |
| `ContinueButtonVariant` | `MButtonVariant.Default` |

---

## Decision Table: Which Base Class?

| Scenario | Use |
|----------|-----|
| Main application window | `WindowModel` (via `MAppOptions.MainModel`) |
| Standalone window (settings, about) | `WindowModel` + `ShowDialogAsync` |
| Confirmation / alert needing OK/Cancel | `DialogModel` |
| Simple alert / confirm (no custom UI) | `MAlert.ShowDialogAsync` / `MConfirm.ShowDialogAsync` |
| Form requiring explicit submit | `DialogModel` |
| Sub-view / panel / region content | `ViewModel` |
| Tab managed by RegionManager | `RegionModel` |

---

## @ViewAttribute + View Source Generator

The `[View<T>]` attribute connects a ViewModel to its View. At build time, the **ViewGenerator**
(Roslyn incremental generator) generates:

- A `CreateView()` override that instantiates the View
- A typed `GetView()` accessor

```csharp
[View<MyView>]   // T = the MView subclass
public sealed partial class MyView_Model : ViewModel { }
```

> The ViewModel class **must be `partial`** for the generator to emit code.

View AXAML — always use `x:DataType` for compiled bindings:

```xml
<MView x:Class="MyApp.Views.MyView"
    xmlns="https://github.com/avaloniaui"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="clr-namespace:MyApp.Views"
    x:DataType="local:MyView_Model">
    <Design.DataContext>
        <local:MyView_Model />
    </Design.DataContext>
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

---

## ViewContext — Parent-to-Child Data Sharing

`SetContext` / `GetContext` pass data through the visual tree without manual property threading.

**Parent provides context:**

```csharp
[View<ParentView>]
public sealed partial class ParentView_Model : ViewModel
{
    public ReactiveValue<bool> TestBool { get; } = new(true);
    public static string Data1 => "Hello, World!";

    public ParentView_Model()
    {
        SetContext(nameof(TestBool), TestBool);
        SetContext(nameof(Data1), Data1);
    }
}
```

**Child consumes context:**

```csharp
[View<ChildView>]
public sealed partial class ChildView_Model : ViewModel
{
    ViewContext<string> _dataCtx => GetContext<string>("Data1");

    public string DataValue => _dataCtx.Value ?? string.Empty;
}
```

`ViewContext<T>` is a `Model` subclass — resolution happens at view activation time by walking up
the parent `MView` chain.

---

## ViewManager — Single-Slot View Switcher

Manages **one active ViewModel at a time** — simpler than `RegionManager` for swapping a content
area without tabs.

```csharp
public sealed partial class ShellView_Model : WindowModel
{
    public ViewManager Content { get; } = new();

    public CommandWarper ShowHomeCommand => GetCommand(() =>
        Content.OpenView(() => new HomeView_Model())
    );

    public CommandWarper ShowSettingsCommand => GetCommand(() =>
        Content.OpenView<SettingsView_Model>()
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
| `OpenView<T>(Func<T> create)` | Open with factory |
| `OpenView<T>(Action<T>? configure)` | Open via service locator, optional post-config |
| `OpenView<T>(Type type, Action<T>? configure)` | Open subclass typed as T |
| `ClearView()` | Clear and dispose current view |
| `Current` | Currently active ViewModel (bindable) |

### MViewHost Data Targeting

Use `MViewHost.ViewModel` when the parent owns which child ViewModel is displayed. If the hosted
child also needs external input from the parent, bind `MViewHost.Data` and implement
`IViewHostTarget<T>` on the child ViewModel.

```csharp
[View<DetailView>]
public sealed partial class DetailView_Model : ViewModel, IViewHostTarget<string>
{
    public string CurrentId
    {
        get;
        set => SetValue(ref field, value);
    } = string.Empty;

    public void SetData(string data)
    {
        CurrentId = data;
    }
}
```

```xml
<MViewHost Data="{Binding SelectedId}">
    <MViewHost.ViewModel>
        <local:DetailView_Model />
    </MViewHost.ViewModel>
</MViewHost>
```

`MViewHost` calls `SetData(...)` whenever `ViewModel` or `Data` changes. Use this when the parent
controls selection or context and the child should react without owning navigation state. Keep the
runtime `Data` type aligned with `IViewHostTarget<T>` or `SetData` will throw.

---

## ViewTemplate — IDataTemplate for Dynamic Views

Use `ViewTemplate<T>` when you need an `IDataTemplate` that creates views from data objects
(e.g., in `ItemsControl`, `ContentControl`).

```csharp
public IDataTemplate ItemTemplate { get; } =
    new ViewTemplate<Document>(doc => new DocumentView_Model { Document = doc });
```

```xml
<ItemsControl ItemsSource="{Binding Documents}"
              ItemTemplate="{Binding ItemTemplate}" />
```

Non-generic form (always creates the same ViewModel type):

```csharp
new ViewTemplate(() => new MyView_Model())
```

---

## RegionModel — Tab / Region ViewModel

Use for views managed as tabs or swappable panels within a `RegionManager`. Inherits `ViewModel`,
adds a `Close()` method.

```csharp
// App-specific abstract base
public abstract class HeaderRegionModel : RegionModel
{
    public abstract string Header { get; }
    public abstract IconSource Icon { get; }
}

// Concrete region
[View<HomeView>]
public sealed partial class HomeView_Model : HeaderRegionModel
{
    public override string Header => LangKey.Overview;
    public override IconSource Icon => Icons.Home;

    protected override void OnClosed() { /* cleanup */ }
}
```

---

## RegionManager — Tab Navigation

`RegionManager<T>` manages a list of `RegionModel` instances with observable selection.

```csharp
// Owned by a service
public sealed class ViewService
{
    public RegionManager<HeaderRegionModel> RegionManager { get; } = new();

    public ViewService(ConfigService config)
    {
        RegionManager.Open(() => new HomeView_Model(), IsCurrent);
        RegionManager.Open(() => new SettingsView_Model(), IsCurrent);
    }

    bool IsCurrent(HeaderRegionModel model) =>
        string.Equals(_config.Config.Current, model.Header);
}
```

**Exposed in WindowModel:**

```csharp
[View<MainView>]
public sealed partial class MainView_Model : WindowModel
{
    readonly ViewService _viewService = Service.GetService<ViewService>();

    public RegionManager<HeaderRegionModel> RegionManager => _viewService.RegionManager;

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

**AXAML wiring:**

```xml
<Grid RowDefinitions="Auto *">
    <MTabTitleBar Grid.Row="0"
                  ItemsSource="{Binding RegionManager.Items}"
                  SelectedItem="{Binding RegionManager.SelectedItem}">
        <MTabTitleBar.ItemTemplate>
            <DataTemplate>
                <MPanel Mode="Horizontal" Spacing="6">
                    <MIcon IconSize="16" IconSource="{Binding Icon}"
                           Margin="0 -10" VerticalAlignment="Center" />
                    <TextBlock Text="{Binding Header}" VerticalAlignment="Center" />
                </MPanel>
            </DataTemplate>
        </MTabTitleBar.ItemTemplate>
        <MTabTitleBar.RightContent>
            <!-- Toolbar buttons -->
        </MTabTitleBar.RightContent>
    </MTabTitleBar>

    <MBackground Grid.Row="1">
        <MViewHost ViewModel="{Binding RegionManager.SelectedItem}" />
    </MBackground>
</Grid>
```

**RegionManager API:**

| Method | Description |
|--------|-------------|
| `Open(Func<T> create)` | Open or activate existing region of that type |
| `Open(Func<T> create, Func<T, bool> isCurrent)` | Open with initial selection predicate |
| `Open<TContext>(TContext ctx, Func<TContext, T> create)` | Open with context key for dedup |
| `Close(T region)` | Close and dispose a region |
| `Items` | Observable collection of regions |
| `SelectedItem` | Currently selected region (bindable) |

---

## Service Registration

```csharp
// Register (in App.axaml.cs OnInitialize)
var configService = Service.Register(new ConfigService(path));
Service.Register(new ViewService(configService));

// Resolve (anywhere)
var config = Service.GetService<ConfigService>();
```

Services are registered as singletons in a static container. Register during `OnInitialize`
before creating any ViewModels.
