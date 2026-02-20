# Project Setup Guide

## MVVM Rules

- **All logic lives in ViewModels.** Views (AXAML + code-behind) contain zero business logic.
- Code-behind only calls `InitializeComponent()`. No event handlers, no manipulation of controls.
- State, commands, and navigation are all exposed via ViewModel properties bound in AXAML.
- Use compiled bindings (`x:DataType`) — always set `Design.DataContext` for design-time support.

## Minimum Project Structure

```
MyApp/
├── MyApp.csproj
├── App.axaml
├── App.axaml.cs
└── Views/
    ├── MainView_Model.cs   # ViewModel
    ├── MainView.axaml      # View
    └── MainView.axaml.cs   # Code-behind (InitializeComponent only)
```

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
    <!-- Manta core (required) -->
    <PackageReference Include="Manta" Version="*" />
    <!-- Optional extension packages -->
    <!-- <PackageReference Include="Manta.DataGrid" Version="*" /> -->
    <!-- <PackageReference Include="Manta.Charts" Version="*" /> -->
    <!-- <PackageReference Include="Manta.NodeEditor" Version="*" /> -->
  </ItemGroup>
</Project>
```

> Check NuGet for the exact package IDs and latest versions.

## App.axaml

```xml
<MApp xmlns="https://github.com/avaloniaui"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      x:Class="MyApp.App"
      RequestedThemeVariant="Default">
  <MApp.Styles>
    <MantaTheme />
    <!-- Add extension themes matching referenced packages -->
    <!-- <MantaDataGridTheme /> -->
    <!-- <MantaChartsTheme /> -->
  </MApp.Styles>
</MApp>
```

`RequestedThemeVariant`: `Default` (follows OS) · `Light` · `Dark`

## App.axaml.cs

```csharp
using Avalonia.Markup.Xaml;
using Manta;
using Manta.Reactive.Services;
using MyApp.Views;

namespace MyApp;

sealed class App : MApp
{
    public App()
    {
        Name = "MyApp";
        SupportActivatableLifetime = true;
    }

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    protected override void OnInitialize(out MAppOptions options)
    {
        // Register all services here, before options
        var configService = Service.Register(new ConfigService());

        options = new()
        {
            MainModel = () => new MainView_Model(),
            Exit = () => configService.Save()
        };
    }
}
```

## MainView_Model.cs — ViewModel

```csharp
using Manta.Reactive.Views;

namespace MyApp.Views;

[View<MainView>]
public sealed class MainView_Model : ViewModel
{
    public static MainView_Model Design => new();

    public string Greeting { get; set => SetValue(ref field, value); } = "Hello, Manta!";

    public CommandWarper SayHelloCommand => GetCommand(() =>
    {
        Greeting = "Button clicked!";
    });
}
```

## MainView.axaml — View

```xml
<MView x:Class="MyApp.Views.MainView"
       xmlns="https://github.com/avaloniaui"
       xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
       xmlns:local="clr-namespace:MyApp.Views"
       x:DataType="local:MainView_Model"
       Design.DataContext="{x:Static local:MainView_Model.Design}">

  <MPanel Mode="Vertical" Spacing="12" Margin="16">
    <TextBlock Text="{Binding Greeting}" FontSize="20" FontWeight="SemiBold" />
    <MButton Content="Say Hello" Command="{Binding SayHelloCommand}" />
  </MPanel>
</MView>
```

## MainView.axaml.cs — Code-behind

```csharp
using Manta.Controls;

namespace MyApp.Views;

public sealed partial class MainView : MView
{
    public MainView() => InitializeComponent();
    // No other code here — all logic goes in the ViewModel
}
```

## MAppOptions — All Options

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

## Theme Switching at Runtime

```csharp
// Toggle between light and dark
MApp.SwitchTheme();

// Subscribe to theme changes (in OnViewActivated)
MApp.SubscribeThemeChanged(variant => { /* e.g. update icon */ })
    .DisposeWith(disposables);
```
