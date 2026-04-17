# Reactive & DynamicData Patterns

Manta's reactive layer is built around four pieces:

- `Model` for property change notification via `SetField(...)`
- `CommandWarper` for sync / async commands with `IsExecuting`
- `ViewModel.OnViewActivated(CompositeDisposable)` for subscription lifetime
- `DynamicData` for live collection pipelines and UI-facing read-only lists

This stack favors explicit state, explicit disposal, and dispatcher-safe collection updates.

---

## Core Reactive Rules

### 1. State lives on `Model`

All reactive state starts with `Model` and `SetField(...)`.

```csharp
public string Query
{
    get;
    set => SetField(ref field, value);
} = string.Empty;
```

`SetField(...)` raises `PropertyChanged` only when the value actually changes.

### 2. Commands live on `CommandModel`

Use `GetCommand(...)` for every command. It caches by property name, so the property can be
evaluated repeatedly without allocating new command instances.

```csharp
public bool CanSave
{
    get;
    set
    {
        if (SetField(ref field, value))
            SaveCommand.OnChanged();
    }
}

public CommandWarper SaveCommand => GetCommand(
    async () => await SaveAsync(),
    () => CanSave
);
```

Use `OnChanged()` whenever `CanExecute` depends on external state.

### 3. Subscriptions live in `OnViewActivated`

`MView` creates a fresh `CompositeDisposable` on attach and disposes it on detach. `ViewModel`
receives that lifetime through `OnViewActivated(...)`.

```csharp
protected override void OnViewActivated(CompositeDisposable disposables)
{
    MApp.SubscribeThemeChanged(_ => RefreshTheme())
        .DisposeWith(disposables);

    GetView()
        .GetWindowStatusObservable()
        .Subscribe(status => CurrentStatus = status)
        .DisposeWith(disposables);
}
```

Use `OnInitialized()` for one-time setup. Use `OnViewActivated(...)` for subscriptions, UI event
bridges, and anything tied to visual-tree lifetime.

### 4. UI-facing observable pipelines switch to dispatcher

Any observable that updates Avalonia-bound state or collections should marshal back to the UI
thread with `ObserveOnDispatcher()`.

```csharp
source
    .ObserveOnDispatcher()
    .Subscribe(x => Current = x)
    .DisposeWith(disposables);
```

`Manta.Reactive.Extensions.ObservableExtensions.ObserveOnDispatcher()` wraps
`AvaloniaScheduler.Instance`.

---

## DynamicData Collection Pattern

Manta uses `DynamicData` as the standard way to build mutable live collections for the UI.

### Preferred shape

- Keep a mutable source inside the ViewModel or manager: `SourceList<T>` or `SourceCache<T, TKey>`
- Project it into a `ReadOnlyObservableCollection<T>`
- Bind AXAML to the read-only collection

```csharp
readonly SourceList<ItemModel> _source = new();

public ReadOnlyObservableCollection<ItemModel> Items { get; }

public MyView_Model()
{
    _source.Connect()
        .Synchronize()
        .ObserveOnDispatcher()
        .Bind(out var items)
        .Subscribe();

    Items = items;
}
```

This is the same pattern used by `RegionManager<T>`.

### Rules for DynamicData in Manta

- Keep `_source` private
- Expose `ReadOnlyObservableCollection<T>` to the view
- Call `ObserveOnDispatcher()` before `Bind(...)` or before UI-bound mutation
- Use `Synchronize()` for collection pipelines that may be touched across threads
- Keep the terminal `Subscribe()` alive for the lifetime of the owner

### Filtering an existing observable collection

When the source is already an observable collection, Manta uses `ToObservableChangeSet()`.

```csharp
_regionManager.Items
    .ToObservableChangeSet()
    .Synchronize()
    .ObserveOnDispatcher()
    .Filter(x => x.Header != LangKey.Overview)
    .Bind(out var items)
    .Subscribe();
```

This is the pattern used in `MantaDemo/Views/OverviewView_Model.cs`.

---

## RegionManager Is the Canonical Dynamic Tab Pattern

`RegionManager<T>` packages the recommended Manta approach for dynamic tab / document / region UIs.

```csharp
readonly RegionManager<MyRegionModel> _regions = new();

public ReadOnlyObservableCollection<MyRegionModel> Items => _regions.Items;

public MyRegionModel? SelectedItem
{
    get => _regions.SelectedItem;
    set => _regions.SelectedItem = value;
}
```

### Behavior

- Internal storage: `SourceList<T>`
- UI projection: `ReadOnlyObservableCollection<T>`
- Selection: `SelectedItem`
- Duplicate suppression: `Open<TTabModel, TContext>(...)` can reuse an existing item via
  `IRegionTarget<TContext>`
- Close behavior: closing the current item selects the previous one, or the first remaining item

### Reopen-by-context pattern

```csharp
public sealed class DocumentRegion_Model : RegionModel, IRegionTarget<string>
{
    public string Path
    {
        get;
        set => SetField(ref field, value);
    } = string.Empty;

    public bool IsTarget(string context) => Path == context;
}

Regions.Open("README.md", path => new DocumentRegion_Model
{
    Path = path
});
```

Use `RegionManager<T>` whenever the UI needs open/reuse/close semantics.

## Reactive Utilities in the Framework

### `ReactiveValue<T>`

Lightweight mutable wrapper for cases where a reference-stable observable value is useful across
multiple ViewModels.

```csharp
public ReactiveValue<bool> Enabled { get; } = new(true);
```

### `ObservableEvent.FromHandler(...)`

Bridge CLR events into `IObservable<T>`.

```csharp
ObservableEvent.FromHandler<TappedEventArgs>(
    x => control.DoubleTapped += x,
    x => control.DoubleTapped -= x
)
```

### Avalonia `GetObservable(...)`

Use Avalonia property observables for control and window state.

```csharp
window.GetObservable(Window.TitleProperty)
    .Subscribe(title => CurrentTitle = title)
    .DisposeWith(disposables);
```

---

## Implementation Guidance for AI Tasks

- Prefer `ViewModel`, `WindowModel`, `DialogModel`, and `RegionModel` over introducing a separate
  reactive framework.
- Prefer `CommandWarper` over `ReactiveCommand`.
- Prefer `OnViewActivated(CompositeDisposable)` for subscriptions and event wiring.
- Prefer `DynamicData` for any collection that supports add/remove/filter/sort and is shown in UI.
- Prefer `ReadOnlyObservableCollection<T>` as the public collection surface.
- Prefer `ReactiveValue<T>` only when nested ViewModels need to mutate the same scalar object.
- Prefer `ObserveOnDispatcher()` before binding collection pipelines or touching UI-bound state.
- Prefer `RegionManager<T>` for tabbed or reusable region content.

### Avoid

- Long-lived subscriptions created in constructors
- Mutable `ObservableCollection<T>` exposed as the write surface
- Recreating commands on every property access
- Updating Avalonia-bound collections from background threads

Build new reactive features in the same style as `RegionManager<T>`, `OverviewView_Model`, and the
`ReactiveDemos` examples.
