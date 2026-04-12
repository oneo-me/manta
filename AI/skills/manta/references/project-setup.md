# Project Setup & Application Framework

## MVVM Rules

- **All logic lives in ViewModels.** Views contain zero business logic.
- Code-behind only calls `InitializeComponent()`. No event handlers, no control manipulation.
- State, commands, and navigation are exposed via ViewModel properties bound in AXAML.
- Use compiled bindings (`x:DataType`) — always set `Design.DataContext` for design-time support.

---

## Minimum Project Structure

```
MyApp/
├── MyApp.csproj
├── App.axaml
├── App.axaml.cs
├── Program.cs
├── Langs/
│   └── en.yml
└── Views/
    ├── MainView_Model.cs   # ViewModel
    ├── MainView.axaml       # View
    └── MainView.axaml.cs    # Code-behind (InitializeComponent only)
```

---

## MyApp.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <AvaloniaUseCompiledBindingsByDefault>true</AvaloniaUseCompiledBindingsByDefault>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Manta" Version="*" />
  </ItemGroup>

  <!-- Include Manta generators (Lang + View source generators) -->
  <Import Project="..\Generators.props" />
</Project>
```

---

## Program.cs — Entry Point

```csharp
static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return MApp.Build<App>();
    }
}
```

`MApp.Build<T>()` pre-configures:
- Platform detection (Windows / macOS / Linux)
- System fonts per platform (PingFang SC on macOS, Microsoft YaHei UI on Windows, Noto Sans CJK SC on Linux)
- Avalonia logger integration

---

## App.axaml

```xml
<MApp xmlns="https://github.com/avaloniaui"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      x:Class="MyApp.App"
      RequestedThemeVariant="Default">
  <MApp.Styles>
    <MantaTheme />
    <!-- App-specific style overrides (optional) -->
    <!-- <StyleInclude Source="Styles/@Styles.axaml" /> -->
  </MApp.Styles>
</MApp>
```

`RequestedThemeVariant`: `Default` (follows OS) · `Light` · `Dark`

---

## App.axaml.cs

```csharp
using Avalonia.Markup.Xaml;
using Manta;
using Manta.Globalization;
using Manta.Logging;
using Manta.Reactive.Services;
using MyApp.Langs;
using MyApp.Views;

namespace MyApp;

sealed class App : MApp
{
    public App()
    {
        Name = "MyApp";
        SupportActivatableLifetime = true;   // macOS activation support
        // SupportMultipleInstances = false;  // enforce single instance (optional)
    }

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    protected override void OnInitialize(out MAppOptions options)
    {
        // 1. Register language data
        Lang.Register(LangData.LoadFromAssembly(typeof(App).Assembly));

        // 2. Set up logging
        Log.AddLogger(new FileLogger(Path.Combine(MApp.Folder, "logs.log")));

        // 3. Register services
        var configService = Service.Register(new ConfigService(configPath));
        Service.Register(new ViewService(configService));

        // 4. Restore language preference
        Lang.Current = configService.Config.Language;

        // 5. Configure app options
        options = new()
        {
            MainModel = () => new MainView_Model(),
            Exit = () => configService.Save()
        };
    }
}
```

---

## MApp API

| Member | Type | Description |
|--------|------|-------------|
| `Current` | `MApp` (static) | Singleton instance |
| `Name` | `string` | Application name |
| `Icon` | `Bitmap?` | Application icon |
| `SupportActivatableLifetime` | `bool` | macOS activation support |
| `SupportMultipleInstances` | `bool` | Allow multiple instances (default: true) |
| `Activate()` | `void` | Focus / show main window |
| `Exit()` | `void` | Shutdown application |
| `SwitchTheme()` | `ThemeVariant` (static) | Toggle light ↔ dark |
| `SubscribeThemeChanged(Action<ThemeVariant?>)` | `IDisposable` (static) | Observe theme changes |

### MApp.Static — Process Information

```csharp
MApp.File    // string — executable path
MApp.Folder  // string — executable directory
MApp.CommandLineArgs  // ReadOnlyCollection<string>
```

---

## MAppOptions

```csharp
options = new MAppOptions
{
    // Main view model factory (required)
    MainModel = () => new MainView_Model(),

    // System tray (optional)
    Tray = new MAppTrayOptions
    {
        Model = () => new TrayView_Model(),
        Icon = Assets.GetBytes("TrayIcon.png")
    },

    // Cleanup on exit (optional)
    Exit = () => { /* save state, flush services */ }
};
```

---

## Theme Switching

```csharp
// Toggle between light and dark
MApp.SwitchTheme();

// Subscribe to theme changes (in OnViewActivated)
MApp.SubscribeThemeChanged(variant => { /* update icons, etc. */ })
    .DisposeWith(disposables);
```

---

## XmlnsDefinition — Namespace Mapping

Manta maps its C# namespaces to the Avalonia global XML namespace via `[assembly: XmlnsDefinition]`.
This is why no extra `xmlns:` prefix is needed for any Manta type:

```csharp
// Manta/Properties/AssemblyInfo.cs
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "Manta")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "Manta.Controls")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "Manta.Langs")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "Manta.Themes")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "Manta.Themes.Variables")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "Manta.Animation")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "Manta.Ava")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "Manta.Globalization.Markups")]
```

When creating **extension libraries**, do the same in your `AssemblyInfo.cs`:

```csharp
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "MyLib.Controls")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "MyLib.Themes")]
```

---

## Service Registration

```csharp
// Register (returns the instance)
var config = Service.Register(new ConfigService(path));
Service.Register(new ViewService(config));

// Resolve
var config = Service.GetService<ConfigService>();

// Create instance with service injection
var vm = Service.CreateInstance<MyViewModel>();
```

Services are thread-safe singletons in a static container. Register during `OnInitialize`
before creating any ViewModels.

---

## Logging System

### Log — Static API

```csharp
Log.Verbose("Detail: {0}", value);
Log.Debug("Processing item {0}", id);
Log.Information("App started");
Log.Warning("Config not found, using defaults");
Log.Error("Failed to load: {0}", ex.Message);
Log.Fatal("Unrecoverable error: {0}", ex);
```

### LogLevel (bitwise flags)

| Level | Flag | Description |
|-------|------|-------------|
| `Verbose` | 1 | Detailed tracing |
| `Debug` | 2 | Debugging info |
| `Information` | 4 | Normal events |
| `Warning` | 8 | Potential issues |
| `Error` | 16 | Errors |
| `Fatal` | 32 | Unrecoverable |
| `Important` | Info \| Warning \| Error \| Fatal | Production default |
| `All` | all flags | Everything |

### Built-in Loggers

```csharp
// Console (color-coded output)
Log.AddLogger(new ConsoleLogger(LogTimeType.Time, LogLevel.All));

// File (thread-safe, auto-creates directories)
Log.AddLogger(new FileLogger("logs/app.log", LogTimeType.DateTime, LogLevel.Important));
```

**LogTimeType:** `None` · `Time` ("HH:mm:ss.fff") · `DateTime` ("yyyy-MM-dd HH:mm:ss.fff")

---

## Assets — Resource Loading

```csharp
var assets = new Assets(typeof(App).Assembly);

// Load resources
Stream stream = assets.GetStream("Images/logo.png");
string text = assets.GetString("Config/default.json");
Bitmap bitmap = assets.GetBitmap("Images/splash.png");
byte[] bytes = assets.GetBytes("Icons/tray.png");
IconSource icon = assets.GetIcon("Icons/save.svg");

// Check existence
bool exists = assets.Exists("Images/logo.png");

// Enumerate
IEnumerable<string> allIcons = assets.GetAssets("Icons");
```

---

## UIThread — Dispatcher Utilities

Thread-safe access to the Avalonia UI thread:

```csharp
// Check if on UI thread
bool isUI = UIThread.CheckAccess();

// Post (fire-and-forget)
UIThread.Post(() => StatusText = "Done");

// Invoke (blocking)
var result = UIThread.Invoke(() => ComputeOnUI());

// Invoke async
await UIThread.InvokeAsync(() => UpdateUI());
await UIThread.InvokeAsync(async () => await LoadAndDisplayAsync());
```

---

## Single Instance Enforcement

Set `SupportMultipleInstances = false` in the `MApp` constructor:

```csharp
sealed class App : MApp
{
    public App()
    {
        Name = "MyApp";
        SupportMultipleInstances = false;  // enforce single instance
    }
}
```

Mechanism:
- Uses a `Mutex` to detect duplicate processes
- Named pipes for IPC — when a second instance launches, its command-line args are forwarded
  to the first instance via the `ReOpen` event
- The second instance exits automatically

---

## Build System — Generators.props

The `Generators.props` file at the solution root provides:

1. **AdditionalFiles** — includes all `Langs/*.yml` files as source generator input
2. **YamlDotNet** — build-time only dependency for YAML parsing
3. **Manta.Generators** — registered as Roslyn analyzer/generator

```xml
<!-- Generators.props (simplified) -->
<ItemGroup>
    <AdditionalFiles Include="Langs\*.yml" />
</ItemGroup>
<ItemGroup>
    <PackageReference Include="YamlDotNet" PrivateAssets="all" ExcludeAssets="runtime" />
    <ProjectReference Include="Manta.Generators.csproj"
        OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
</ItemGroup>
```

Import in your `.csproj`:

```xml
<Import Project="..\Generators.props" />
```

### View Generator

The **ViewGenerator** automatically implements `CreateView()` for any `partial` class annotated
with `[View<T>]`. No manual wiring needed.

### Lang Generator

The **LangGenerator** reads `Langs/*.yml` files and generates `LangKey` (constants) and `LangData`
(runtime dictionary). See [i18n.md](i18n.md) for details.
