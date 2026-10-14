# Manta architecture

Manta is a .NET 10 library that layers a themeable control set, an MVVM runtime and compile-time generators on top of Avalonia 12. `MantaDemo`, `Manta.Generators` and `MantaPrepare` support the library rather than ship as its API.

## Code map

All .NET projects and their build files live under `source/`; standalone samples that consume the published packages live under `samples/`. The app icon is `source/icon.ico` and `source/icon.png`, while the `icon.icon` design project stays at the root.

| Path | Responsibility |
| --- | --- |
| `source/Manta/Controls` | `M*` controls, `MView`/`MViewHost`, and the base control themes under `Base/`. |
| `source/Manta/Themes` | OKLch color values, theme variable markup extensions, and the generated `MantaTheme` dictionary. |
| `source/Manta/Reactive` | MVVM runtime: models, commands, service locator and regions. |
| `source/Manta/Globalization` | Language lookup, `LangKey`/`LangData` API and `{Lang}` markup extensions. |
| `source/Manta/Animation` | `SmoothBrushTransition` for brush interpolation. |
| `source/Manta/Logging`, `source/Manta/Ava`, `source/Manta/Media`, `source/Manta/Extensions` | Logging, Avalonia helpers, image sources and utilities. |
| `source/Manta.Generators` | Roslyn generators for `[View<T>]` plumbing and language key/data classes. |
| `source/MantaPrepare` | Tool that regenerates control theme stubs, `MantaTheme.axaml` and demo screens. |
| `source/MantaDemo` | Reference application and control showcase. |
| `samples/MantaSample` | Standalone app that installs `ONEO.Manta` from NuGet and shows the main features. |
| `samples/MantaComponents` | Plain Avalonia app that installs `ONEO.Manta` from NuGet and uses only its theme and controls. |
| `samples/AvaloniaI18nSample` | Stock Avalonia app that installs `ONEO.Manta` for the localization feature only. |
| `website` | Marketing site at the repository root, built with React Router and shadcn/ui; deployed to GitHub Pages. |

## Boundaries

- The `Manta` library never references `MantaDemo`; the demo references the library.
- The samples under `samples/` reference the published packages, never the `source/` projects.
- `Manta.Generators` targets `netstandard2.0` and is consumed as an analyzer through `Generators.props`.
- Name controls `M` + the Avalonia control name, and give each control its own style key.
- Never hand-edit generated files: `MantaTheme.axaml`, `Controls/Base/Supported`, `Controls/Base/Unsupported` and `MantaDemo/Demos/AutoDemos`. Regenerate them with `MantaPrepare`.
- Express colors and spacing through theme variables (`MBrush`, `MSize`, and similar). Do not hardcode visual values in controls.
- Keep view models free of control references. They expose bindings and commands; views own layout.
- Route every user-facing string through `LangKey` and the `{Lang}` markup instead of a literal.
- Build with `TreatWarningsAsErrors` and keep the public API AOT-compatible.

## Core flows

- **Startup.** `Program` calls `MApp.Build<T>()`. `MApp.OnFrameworkInitializationCompleted` handles single-instance checks and registers default menus.
- **Initialization.** `OnInitialize` collects `MAppOptions`; `MAppLifetime` asks the main `WindowModel` for its window and assigns it as the desktop main window.
- **View materialization.** `ViewModelExtensions.BuildView` calls the generated `CreateView`, sets the data context and caches the view. `MView` activates the view model when it attaches to the visual tree; `MViewHost.ViewModel` builds a view on assignment.
- **Region navigation.** `RegionManager<T>.Open` reuses a `RegionModel` that matches the context through `IRegionTarget<T>`, or creates one, then updates `SelectedItem`.
- **Language resolution.** The `LangGenerator` reads `Langs/*.yml` and emits `LangKey` constants and `LangData.Load()`. Bindings created by `{Lang}` subscribe to `Lang.Current` and refresh when it changes.
- **Code generation.** `MantaPrepare` scans `Design.PreviewWith` blocks and control files, then rewrites the theme dictionary and demo screens.

## Data and state

- Manta keeps no database and no runtime state on disk; themes and translations are compiled into the assembly.
- `MantaDemo` writes `config.json` under `~/.manta` and `logs.log` under `~/.oneo/MantaDemo`.
- `Service` is a process-wide static registry with no scopes or disposal; register services during `OnInitialize`.

## Verification

- There is no test project, so `dotnet test` runs nothing.
- The language generator emits `LANG001` errors and `LANG002` warnings for parse failures, missing keys and redundant keys.
- Verify behavior by running `MantaDemo` or the applications under `samples/` and exercising the affected screen. `samples/MantaComponents` covers the controls-and-theme path without the MVVM runtime.

## Subjects

| Document | When to read |
| --- | --- |
| [docs/architecture/mvvm-and-views.md](docs/architecture/mvvm-and-views.md) | Changing view models, view hosting, commands, services or regions. |
| [docs/architecture/theming.md](docs/architecture/theming.md) | Changing colors, theme resources, control themes or `MantaPrepare` output. |
| [docs/architecture/localization.md](docs/architecture/localization.md) | Adding a language, a translation key or changing the language generator. |
