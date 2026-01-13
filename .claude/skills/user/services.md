# Services Guide

This guide covers the Manta service locator pattern for dependency management.

## Overview

Manta provides a simple service locator (`Service`) for registering and resolving dependencies. Services are registered at application startup and can be injected into ViewModels via constructor parameters.

## Service Registration

Register services in `App.axaml.cs` during initialization:

```csharp
public override void OnFrameworkInitializationCompleted()
{
    // Register interface with implementation
    Service.Register<IConfigService, ConfigService>(new ConfigService());

    // Register concrete type
    Service.Register(new LogService());

    // Chain registrations
    var dataService = Service.Register<IDataService, DataService>(new DataService());

    base.OnFrameworkInitializationCompleted();
}
```

### Registration Patterns

```csharp
// Pattern 1: Interface + Implementation
Service.Register<IConfigService, ConfigService>(new ConfigService());

// Pattern 2: Concrete type only
Service.Register(new LogService());

// Pattern 3: With initialization
var config = new ConfigService();
config.Load();
Service.Register<IConfigService, ConfigService>(config);
```

## Service Resolution

### In ViewModels (Constructor Injection)

ViewModels can receive services via constructor injection:

```csharp
[View<SettingsView>]
sealed class SettingsView_Model : ViewModel
{
    readonly IConfigService _config;
    readonly IDataService _data;

    // Services are automatically injected
    public SettingsView_Model(IConfigService config, IDataService data)
    {
        _config = config;
        _data = data;
    }
}
```

### Direct Resolution

```csharp
// Get registered service
var config = Service.GetService<IConfigService>();
var log = Service.GetService<LogService>();
```

### Manual Instance Creation

```csharp
// Create instance with injected dependencies
var viewModel = Service.CreateInstance<MyViewModel>();

// Create from type
var viewModel = Service.CreateInstance<ViewModel>(typeof(SettingsView_Model));
```

## Defining Services

### Interface-Based Service

```csharp
// Interface
public interface IConfigService
{
    string Theme { get; set; }
    void Load();
    void Save();
}

// Implementation
public class ConfigService : IConfigService
{
    public string Theme { get; set; } = "Default";

    public void Load()
    {
        // Load from file
    }

    public void Save()
    {
        // Save to file
    }
}
```

### Concrete Service

```csharp
public class NavigationService
{
    readonly ViewManager _viewManager = new();

    public ViewManager ViewManager => _viewManager;

    public void NavigateTo<T>() where T : ViewModel
    {
        _viewManager.OpenView<T>();
    }
}
```

## Example: Complete Service Setup

### Service Definitions

```csharp
// Services/IConfigService.cs
public interface IConfigService
{
    string Theme { get; set; }
    string Language { get; set; }
    void Save();
}

// Services/ConfigService.cs
public class ConfigService : IConfigService
{
    readonly string _configPath;

    public string Theme { get; set; } = "Light";
    public string Language { get; set; } = "en-US";

    public ConfigService()
    {
        _configPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MyApp",
            "config.json"
        );
        Load();
    }

    void Load()
    {
        if (File.Exists(_configPath))
        {
            var json = File.ReadAllText(_configPath);
            // Deserialize...
        }
    }

    public void Save()
    {
        var directory = Path.GetDirectoryName(_configPath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory!);

        var json = JsonSerializer.Serialize(this);
        File.WriteAllText(_configPath, json);
    }
}
```

### App Registration

```csharp
// App.axaml.cs
public override void OnFrameworkInitializationCompleted()
{
    // Register services
    Service.Register<IConfigService, ConfigService>(new ConfigService());

    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        desktop.MainWindow = ViewManager.OpenWindow<MainWindow_Model>();
    }

    base.OnFrameworkInitializationCompleted();
}
```

### ViewModel Usage

```csharp
[View<SettingsView>]
sealed class SettingsView_Model : ViewModel
{
    readonly IConfigService _config;

    public SettingsView_Model(IConfigService config)
    {
        _config = config;
        _theme = config.Theme;
    }

    string _theme;
    public string Theme
    {
        get => _theme;
        set
        {
            if (SetValue(ref _theme, value))
            {
                _config.Theme = value;
                _config.Save();
            }
        }
    }
}
```

## Best Practices

### 1. Register Early

Register all services in `OnFrameworkInitializationCompleted` before opening any windows.

### 2. Use Interfaces

Prefer interface-based registration for testability:

```csharp
// Good
Service.Register<IDataService, DataService>(new DataService());

// Less flexible
Service.Register(new DataService());
```

### 3. Single Responsibility

Each service should have a focused purpose:

```csharp
// Good - focused services
Service.Register<IConfigService, ConfigService>(...);
Service.Register<INavigationService, NavigationService>(...);
Service.Register<IDialogService, DialogService>(...);

// Avoid - god service
Service.Register<IAppService, AppService>(...);  // Does everything
```

### 4. Avoid Service Locator in Views

Use constructor injection in ViewModels rather than resolving services in code-behind:

```csharp
// Good - constructor injection
public SettingsView_Model(IConfigService config)
{
    _config = config;
}

// Avoid - service locator pattern in view
public SettingsView()
{
    var config = Service.GetService<IConfigService>();  // Harder to test
}
```

### 5. Handle Missing Services

The service system throws `ServiceException` if a service is not registered:

```csharp
try
{
    var service = Service.GetService<IMissingService>();
}
catch (ServiceException ex)
{
    // Handle missing service
}
```

## Common Services

| Service | Purpose |
|---------|---------|
| `IConfigService` | Application settings |
| `INavigationService` | View navigation |
| `IDialogService` | Modal dialogs |
| `IDataService` | Data access |
| `ILogService` | Logging |
| `IAuthService` | Authentication |
