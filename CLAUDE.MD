# LightKit - Project Guide for Claude

## Project Overview

LightKit is a comprehensive UI toolkit and MVVM framework built on Avalonia 11.3.10 for cross-platform desktop applications. It provides a rich set of 40+ themeable controls, a reactive MVVM infrastructure, and a sophisticated theming system with light/dark mode support.

**Technology Stack:**
- .NET 10.0
- Avalonia 11.3.10 (cross-platform UI framework)
- C# with nullable reference types and compiled bindings
- DynamicData 9.4.1 (reactive data streams)
- YamlDotNet 16.3.0 (configuration/data serialization)
- Roslyn source generators for compile-time language key generation

## Project Structure

```
LightKit/                       # Main library project
├── Animation/                  # Custom animations (SmoothBrushTransition)
├── Controls/                   # 40+ custom controls
│   ├── Base/Supported/        # Styled Avalonia controls
│   ├── Base/Unsupported/      # Placeholder themes
│   └── Panels/                # Layout panels (8 types)
├── Themes/                     # Theme system
│   ├── BaseTheme.axaml        # Master theme orchestrator
│   ├── @Light.axaml           # Light theme colors
│   ├── @Dark.axaml            # Dark theme colors
│   └── Values/                # Color definitions (OKLch color space)
├── Reactive/                   # MVVM framework
│   ├── Views/                 # ViewModelBase, ViewModel, WindowModel
│   ├── Commands/              # Command infrastructure
│   ├── Services/              # Service locator pattern
│   └── Extensions/            # Reactive extensions
├── Globalization/              # Multi-language support
│   ├── Core/                  # Language data loading
│   └── Markups/               # XAML markup extensions
├── Logging/                    # Structured logging
└── Extensions/                 # Utility extensions

LightKit.Demo/                  # Reference application
├── Views/                      # Main UI views and view models
├── Demos/                      # Control showcase
│   ├── ControlDemos/          # Individual control examples
│   └── StyleDemos/            # Style/theme demonstrations
└── Services/                   # Config, Demo, ViewService

LightKit.Generators/            # Roslyn source generators
└── Language key generation from YAML

LightKit.Prepare/               # Build preparation utilities
```

## Key Features

### Theming System
- Light/Dark theme support with OKLch color space interpolation
- Centralized `BaseTheme.axaml` (123 lines) orchestrates 100+ style resources
- Smooth color transitions via `SmoothBrushTransition` animation
- Theme-aware color values with fallbacks

### MVVM Framework
- `ViewModel` and `WindowModel` base classes with reactive support
- Attribute-based view registration using `@ViewAttribute`
- `ViewService` for view lifecycle management
- Reactive command infrastructure
- Service locator pattern for dependency management

### Control Library (40+ Controls)

**Layout/Container:**
- LightAccordion, LightCard, LightPanel, LightItems, LightWindow, LightViewHost, LightView

**Input:**
- LightButton, LightCheckBox, LightToggleButton, LightTextBox, LightCommand (command palette)

**Display:**
- LightLabel, LightText, LightIcon, LightPathIcon, LightAvatar, LightBadge, LightBreadcrumb, LightAlert

**Data:**
- LightListBox, LightChart, LightCarousel, LightTabStrip

**Specialized:**
- LightProgressRing, LightRing, LightAspectRatio, LightAutoGrid, LightScrollViewer, LightTitleBarButtons

### Layout Panels
8 custom layout panel types for flexible UI arrangement (Waterfall, Wrap, Auto-grid, Uniform, etc.)

### Globalization
- Multi-language support with YAML-based language files
- XAML markup extensions for translations
- Code generation for type-safe language keys

## Recent Additions

### SmoothBrushTransition (Dec 26, 2024)
Location: `/LightKit/Animation/SmoothBrushTransition.cs`

Smooth color/brush interpolation with premultiplied alpha blending:
- Supports SolidColorBrush and GradientBrush transitions
- Handles opacity correctly using premultiplied alpha
- Applied to: LightButton, LightChart, LightCommand, LightListBox, LightTitleBarButton, LightToggleButton

### LightCommand Control (Dec 23, 2024)
Location: `/LightKit/Controls/LightCommand.cs` and `.axaml`

Command palette / searchable command menu control:
- Properties: Watermark (placeholder), Text (search/input)
- Variants: Default, Separator, Group, NoResults
- Use case: Quick action/command selection interface

### PopupRoot Control (Dec 23, 2024)
Location: `/LightKit/Controls/Base/PopupRoot.axaml`

Root container for popup windows with transparent background and VisualLayerManager integration.

### LightToggleButton (Dec 24, 2024)
Location: `/LightKit/Controls/LightToggleButton.cs` and `.axaml`

Toggle button with variants and size options:
- Variants: Outline, WithText
- Sizes: Default, Small, Large

### LightCheckBox (Dec 22, 2024)
Location: `/LightKit/Controls/LightCheckBox.cs` and `.axaml`

Enhanced checkbox with custom header support:
- Variants: Default, Bordered
- Properties: Header, HeaderTemplate, HeaderPresenter
- Supports custom label/content via templates

## Architecture Patterns

### Control Development
When creating new controls:
1. Inherit from appropriate Avalonia base control or create custom control
2. Define variants using enums (e.g., `LightCheckBoxVariant`)
3. Create corresponding `.axaml` theme file in `/LightKit/Controls/`
4. Register styles in `BaseTheme.axaml`
5. Add demo to `/LightKit.Demo/Demos/ControlDemos/`
6. Apply `SmoothBrushTransition` for color/brush animations where appropriate

### Theme Integration
- All color values use OKLch color space for perceptually uniform interpolation
- Define colors in `/LightKit/Themes/Values/`
- Use `@Light.axaml` and `@Dark.axaml` for theme-specific values
- Reference via `{StaticResource}` or `{DynamicResource}` in XAML

### MVVM Implementation
```csharp
// ViewModel definition
[View<YourView>]
public partial class YourViewModel : ViewModel
{
    // Reactive properties and commands
}

// View registration happens automatically via attribute
```

### Service Registration
```csharp
// Use service locator pattern
Services.Register<IYourService, YourService>();
var service = Services.Get<IYourService>();
```

## Build and Run

**Prerequisites:**
- .NET 10.0 SDK
- JetBrains Rider or Visual Studio 2022+

**Build:**
```bash
dotnet build LightKit.slnx
```

**Run Demo:**
```bash
dotnet run --project LightKit.Demo/LightKit.Demo.csproj
```

## Code Style

- Nullable reference types enabled
- Implicit usings enabled
- Compiled bindings preferred in XAML
- Follow existing naming conventions (Light* prefix for controls)
- Use proper XML documentation for public APIs

## Testing Strategy

- Demo application serves as interactive test suite
- Each control has corresponding demo in `/LightKit.Demo/Demos/ControlDemos/`
- Visual verification through theme demos in `/LightKit.Demo/Demos/StyleDemos/`

## Common Tasks

### Adding a New Control
1. Create control class in `/LightKit/Controls/YourControl.cs`
2. Create theme file `/LightKit/Controls/YourControl.axaml`
3. Register in `/LightKit/Themes/BaseTheme.axaml`
4. Create demo in `/LightKit.Demo/Demos/ControlDemos/YourControlDemo.axaml`
5. Update this documentation

### Adding a New Theme Color
1. Define color in `/LightKit/Themes/Values/`
2. Add light variant to `@Light.axaml`
3. Add dark variant to `@Dark.axaml`
4. Reference in control themes

### Adding Localization Support
1. Add language YAML file to globalization resources
2. Run source generator to generate type-safe keys
3. Use markup extensions in XAML: `{lang:Translate Key=YourKey}`

## Important Files

- `/LightKit/Themes/BaseTheme.axaml` - Master theme orchestrator (123 lines)
- `/Directory.Build.props` - Shared MSBuild properties
- `/LightKit.sln.DotSettings` - ReSharper/Rider settings
- `/LightKit/Animation/SmoothBrushTransition.cs` - Core animation logic

## Git Workflow

Current branch: `live`
Main branch: `main` (use for PRs)

Recent commits demonstrate active control development with focus on interactive controls and smooth animations.

## Notes for AI Assistants

- Always check existing control implementations before creating new ones
- Follow the established pattern: Control class + AXAML theme + Demo
- Use `SmoothBrushTransition` for brush/color animations
- Respect the OKLch color space for theme consistency
- When modifying themes, update both light and dark variants
- The Demo project serves as living documentation - keep it updated
- Source generators run at compile time - rebuild after modifying language files
