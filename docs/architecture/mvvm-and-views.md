# MVVM and views

Manta's MVVM layer is explicit: view models are plain observable objects, views are `MView` controls, and a source generator connects the two. The layer adds no container and no reflection-based view discovery.

## Runtime types

| Type | Role |
| --- | --- |
| `Model` | `INotifyPropertyChanged` base with `OnPropertyChanged` and `SetField`. |
| `CommandModel` | Caches `ICommand` instances per property name through `GetCommand`. |
| `ViewModel` | Adds `CreateView` and the view lifecycle hooks. |
| `WindowModel` | Owns an `MWindow`, with `OnWindowInitialized`, `Close` and `CloseAsync`. |
| `DialogModel` | A window model with `Result` and cancel/continue commands. |
| `RegionModel` | A closable view model that participates in region navigation. |
| `RegionManager<T>` | Observable list of region models with `Open`, `SelectedItem` and `Close`. |
| `MView`, `MViewHost` | Controls that activate a view model and build its view. |
| `ViewTemplate` | `IDataTemplate` wrapping a view model or control factory. |

## View registration

Apply `[View<TView>]` to a `partial` view model that derives from `ViewModel`. `ViewGenerator` emits a `CreateView` override that returns a new `TView`, plus a typed `GetView()` accessor. `TView` must derive from `MView` and have a parameterless constructor.

The view model declares only its state and commands. The view supplies the control tree and its own `x:DataType`; there is no naming convention to follow beyond the attribute.

## Lifecycle

- `ViewModelExtensions.BuildView` calls `CreateView` on first use, assigns the view model as `DataContext`, and caches the result in `InnerView`.
- `MView.OnAttachedToVisualTree` opens a `CompositeDisposable` and calls `OnInnerViewActivated`. `OnInitialized` runs once per view model; `OnViewActivated` runs on every attach and receives the disposables.
- `MView.OnDetachedFromVisualTree` disposes the subscriptions, so re-attaching creates a fresh scope. Subscribe to observables with `DisposeWith(disposables)`.
- `WindowModelExtensions.CreateWindow` wraps the view in an `MWindow`, applies the app icon and title, and centers it on open. `DialogModel` overrides window options for a fixed, sized-to-content dialog.

## Commands

`GetCommand` returns a `CommandWarper` (or `CommandWarper<T>`) and caches it against the caller's property name. Overloads accept synchronous and asynchronous actions and an optional `canAction` predicate. Expose commands as expression-bodied properties so the cache key stays stable.

## Services

`Service` is a static, process-wide registry. `Register` stores a service under both its declared and concrete types and throws `ServiceException` on duplicate registration. `GetService<T>` throws when the type is missing. `MApp` registers `ImageLoadService`; applications register the rest during `OnInitialize`.

## Regions

`RegionManager<T>` holds a `SourceList<T>`, projects it into `Items`, and marshals changes to the UI thread. `Open<TTabModel, TContext>` looks for an existing item of the same type that returns `true` from `IRegionTarget<TContext>.IsTarget`, and reuses it; otherwise it creates one through the supplied factory and selects it.

Selecting and closing are coordinated under a lock. Closing the selected item selects its neighbor, falling back to `null` when the list empties. Dispose the manager to close every item.
