# Manta

**English** | [简体中文](README.zh-CN.md)

Manta is a UI toolkit and MVVM framework for building cross-platform desktop applications on Avalonia. It provides themeable controls, an explicit MVVM runtime, and compile-time code generation.

## Features

- A broad set of `M`-prefixed controls, plus themed styles for supported Avalonia controls.
- Light and dark themes built on the OKLch color space.
- `Model`, `ViewModel`, `WindowModel` and `DialogModel` base classes for the MVVM layer.
- Attribute-based view registration that generates view creation at compile time.
- Region and tab navigation with `RegionManager<T>`.
- Modal dialogs with `DialogModel`.
- Type-safe language keys generated from YAML files.

## Install

Manta targets .NET 10 and Avalonia 12. Packages are published to [NuGet.org](https://www.nuget.org/packages/Manta).

```sh
dotnet add package Manta
dotnet add package Manta.Generators
```

Add `Manta.Generators` to use the `[View<T>]` and language-key generators.

## Usage

Create the application entry point and subclass `MApp`:

```csharp
using Avalonia;
using Manta;

static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        MApp.Build<App>().StartWithClassicDesktopLifetime(args);
    }
}
```

Register the theme and a main window model in the application:

```xml
<MApp xmlns="https://github.com/avaloniaui"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      x:Class="MyApp.App">
    <MApp.Styles>
        <MantaTheme />
    </MApp.Styles>
</MApp>
```

```csharp
sealed class App : MApp
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    protected override void OnInitialize(out MAppOptions options)
    {
        options = new() { MainModel = () => new Main_Model() };
    }
}
```

Pair a view model with its view using the `View` attribute. The generator supplies `CreateView`, so the model only declares its bindings:

```csharp
[View<Main>]
sealed partial class Main_Model : WindowModel
{
}

sealed partial class Main : MView
{
    public Main() => InitializeComponent();
}
```

Read the [architecture overview](ARCHITECTURE.md) before extending the library.

## Documentation

- [ARCHITECTURE.md](ARCHITECTURE.md) maps the projects and the boundaries between them.
- The [architecture subjects](ARCHITECTURE.md#subjects) explain the MVVM runtime, theming and localization in depth.
- [CHANGELOG.md](CHANGELOG.md) lists user-visible changes.

## Development

Install the .NET 10 SDK. The .NET projects live under `source/`; run the commands from there:

```sh
cd source
dotnet build Manta.slnx
dotnet run --project MantaDemo
```

After editing a control's `Design.PreviewWith` block or theme files, regenerate the derived files:

```sh
dotnet run --project MantaPrepare
```

Release by pushing a `v<major>.<minor>.<patch>` tag. The release workflow publishes the packages to NuGet.org and creates the GitHub release, using the `NUGET_API_KEY` repository secret.

Contributor rules live in [AGENTS.md](AGENTS.md).

## License

Manta is released under the [MIT license](LICENSE).
