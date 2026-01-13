# Project Structure Guide

This guide describes the recommended project structure for applications using the Manta UI toolkit.

## Recommended Directory Structure

```
YourApp/
├── App.axaml                    # Application resources
├── App.axaml.cs                 # Application entry point
├── Program.cs                   # Main entry
│
├── Views/                       # Views and ViewModels
│   ├── MainView.axaml           # Main view XAML
│   ├── MainView.axaml.cs        # Main view code-behind
│   ├── MainView_Model.cs        # Main view model
│   ├── SettingsView.axaml
│   ├── SettingsView.axaml.cs
│   ├── SettingsView_Model.cs
│   └── ...
│
├── Services/                    # Application services
│   ├── ConfigService.cs
│   ├── DataService.cs
│   └── ...
│
├── Models/                      # Data models (not ViewModels)
│   ├── User.cs
│   ├── Settings.cs
│   └── ...
│
├── Assets/                      # Static resources
│   ├── Icons/
│   ├── Images/
│   └── Fonts/
│
└── Langs/                       # Localization (optional)
    ├── en-US.yaml
    └── zh-CN.yaml
```

## Naming Conventions

### Files

| Type | Pattern | Example |
|------|---------|---------|
| View XAML | `{Name}View.axaml` | `SettingsView.axaml` |
| View Code-Behind | `{Name}View.axaml.cs` | `SettingsView.axaml.cs` |
| ViewModel | `{Name}View_Model.cs` | `SettingsView_Model.cs` |
| Window ViewModel | `{Name}Window_Model.cs` | `MainWindow_Model.cs` |
| Service | `{Name}Service.cs` | `ConfigService.cs` |
| Data Model | `{Name}.cs` | `User.cs` |

### Classes

| Type | Pattern | Example |
|------|---------|---------|
| View | `{Name}View` | `SettingsView` |
| ViewModel | `{Name}View_Model` | `SettingsView_Model` |
| WindowModel | `{Name}Window_Model` | `MainWindow_Model` |
| Service | `{Name}Service` | `ConfigService` |

## Application Setup

### App.axaml

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="YourApp.App"
             RequestedThemeVariant="Default">
    <Application.Styles>
        <MantaTheme />
    </Application.Styles>
</Application>
```

### App.axaml.cs

```csharp
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Manta.Controls;
using Manta.Reactive.Services;
using Manta.Reactive.Views;

namespace YourApp;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Register services
        Service.Register<IConfigService, ConfigService>(new ConfigService());

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Open main window
            desktop.MainWindow = ViewManager.OpenWindow<MainWindow_Model>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
```

### Program.cs

```csharp
using Avalonia;

namespace YourApp;

class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
```

## Project File (.csproj)

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <OutputType>WinExe</OutputType>
        <TargetFramework>net10.0</TargetFramework>
        <Nullable>enable</Nullable>
        <AvaloniaUseCompiledBindingsByDefault>true</AvaloniaUseCompiledBindingsByDefault>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="Avalonia.Desktop" Version="11.3.0" />
        <PackageReference Include="Avalonia.Fonts.Inter" Version="11.3.0" />
        <PackageReference Include="Manta" Version="*" />
    </ItemGroup>

    <ItemGroup>
        <AvaloniaResource Include="Assets\**" />
    </ItemGroup>
</Project>
```

## View/ViewModel Organization

### Single Module

For simple applications, keep all views in the `Views/` folder.

### Multiple Modules

For larger applications, organize by feature:

```
YourApp/
├── Modules/
│   ├── Auth/
│   │   ├── LoginView.axaml
│   │   ├── LoginView.axaml.cs
│   │   └── LoginView_Model.cs
│   ├── Dashboard/
│   │   ├── DashboardView.axaml
│   │   └── ...
│   └── Settings/
│       └── ...
└── ...
```

## Best Practices

1. **One ViewModel per View** - Each view should have exactly one corresponding ViewModel

2. **Keep Views Thin** - Put logic in ViewModels, not in code-behind

3. **Use Services for Shared Logic** - Don't duplicate logic across ViewModels

4. **Compiled Bindings** - Always use `x:DataType` for type-safe bindings

5. **Design-Time Support** - Add `Design.DataContext` for XAML previewer

```xml
<MView x:DataType="local:MyView_Model"
       Design.DataContext="{x:Static local:MyView_Model.Design}">
```
