# Manta - Project Guide for Claude

## Project Overview

Manta is a comprehensive UI toolkit and MVVM framework built on Avalonia 11.3.10 for cross-platform desktop applications. It provides a rich set of 40+ themeable controls, a reactive MVVM infrastructure, and a sophisticated theming system with light/dark mode support.

**Technology Stack:**
- .NET 10.0
- Avalonia 11.3.10 (cross-platform UI framework)
- C# with nullable reference types and compiled bindings
- DynamicData 9.4.1 (reactive data streams)
- YamlDotNet 16.3.0 (configuration/data serialization)
- Roslyn source generators for compile-time language key generation

## Project Structure

```
Manta/                        # Main library project
├── Animation/                  # Custom animations (SmoothBrushTransition)
├── Controls/                   # 40+ custom controls (M* prefix)
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

Manta.Demo/                   # Reference application
├── Views/                      # Main UI views and view models
├── Demos/                      # Control showcase
│   ├── ControlDemos/          # Individual control examples
│   └── StyleDemos/            # Style/theme demonstrations
└── Services/                   # Config, Demo, ViewService

Manta.Generators/             # Roslyn source generators
└── Language key generation from YAML

Manta.Prepare/                # Build preparation utilities
```

## Key Features

### Theming System
- Light/Dark theme support with OKLch color space interpolation
- Centralized `BaseTheme.axaml` orchestrates 100+ style resources
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
- MExpander, MCard, MPanel, MItems, MWindow, MViewHost, MView, MClipContentControl

**Input:**
- MButton, MCheckBox, MToggleButton, MTextBox, MCommand (command palette), MComboBox

**Display:**
- MIcon, MPathIcon, MAvatar, MBadge, MBreadcrumb, MAlert

**Data:**
- MListBox, MChart, MCarousel, MTabStrip, MTabTitleBar

**Specialized:**
- MProgressRing, MRing, MAspectRatio, MScrollViewer, MTitleBar, MAnimatedExpander, MPixelGrid, MBackgroundBlur

### Layout Panels
8 custom layout panel types for flexible UI arrangement (Waterfall, Wrap, Auto-grid, Uniform, etc.)

### Globalization
- Multi-language support with YAML-based language files
- XAML markup extensions for translations
- Code generation for type-safe language keys

## Architecture Patterns

### Control Development
When creating new controls:
1. Inherit from appropriate Avalonia base control or create custom control
2. Define variants using enums (e.g., `MCheckBoxVariant`)
3. Create corresponding `.axaml` theme file in `/Manta/Controls/`
4. Register styles in `MantaTheme.axaml`
5. **Do NOT create separate Demo files** - The Demo app automatically scans `Design.PreviewWith` sections in control AXAML files
6. Move corresponding Avalonia base control style from `Base/Unsupported/` to `Base/Supported/` with message like `"Please use the 'MYourControl'"`
7. Apply `SmoothBrushTransition` for color/brush animations where appropriate
8. **Update CLAUDE.md** if adding significant new features

### Theme Integration
- All color values use OKLch color space for perceptually uniform interpolation
- Define colors in `/Manta/Themes/Values/`
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
dotnet build Manta.slnx
```

**Run Demo:**
```bash
dotnet run --project Manta.Demo/Manta.Demo.csproj
```

## Code Style

- Nullable reference types enabled
- Implicit usings enabled
- Compiled bindings preferred in XAML
- Follow existing naming conventions (M* prefix for controls)
- Use proper XML documentation for public APIs

## Testing Strategy

- Demo application serves as interactive test suite
- Each control has corresponding demo in `/Manta.Demo/Demos/ControlDemos/`
- Visual verification through theme demos in `/Manta.Demo/Demos/StyleDemos/`

## Common Tasks

### Adding a New Control
1. Create control class in `/Manta/Controls/YourControl.cs`
2. Create theme file `/Manta/Controls/YourControl.axaml` with `Design.PreviewWith` section for auto-demo
3. Register in `/Manta/Themes/MantaTheme.axaml`
4. Move base control from `Base/Unsupported/` to `Base/Supported/` with usage guidance message
5. Update this documentation (CLAUDE.md)

### Adding a New Theme Color
1. Define color in `/Manta/Themes/Values/`
2. Add light variant to `@Light.axaml`
3. Add dark variant to `@Dark.axaml`
4. Reference in control themes

### Adding Localization Support
1. Add language YAML file to globalization resources
2. Run source generator to generate type-safe keys
3. Use markup extensions in XAML: `{lang:Translate Key=YourKey}`

## Important Files

- `/Manta/Themes/BaseTheme.axaml` - Master theme orchestrator
- `/Directory.Build.props` - Shared MSBuild properties
- `/Manta.sln.DotSettings` - ReSharper/Rider settings
- `/Manta/Animation/SmoothBrushTransition.cs` - Core animation logic
- `/CHANGELOG.md` - Project change history
- `/README.md` & `/README.zh-CN.md` - Project documentation (bilingual)

## Git Workflow

Current branch: `live`
Main branch: `main` (use for PRs)

### Commit Convention
- **Claude commits MUST start with `[CLAUDE] `** - e.g., `[CLAUDE] Add MRadioButton control`
- No prefix - Changes made by user

When asked to update documentation, use `git log` to identify recent changes and their authors.

## Notes for AI Assistants

- Always check existing control implementations before creating new ones
- Follow the established pattern: Control class + AXAML theme (with `Design.PreviewWith`)
- **Do NOT create separate ControlDemo files** - Demo app auto-scans `Design.PreviewWith` sections
- Use `SmoothBrushTransition` for brush/color animations
- Respect the OKLch color space for theme consistency
- When modifying themes, update both light and dark variants
- Source generators run at compile time - rebuild after modifying language files
- **Change History**: Use `git log` or `CHANGELOG.md` to view recent changes
- **Commit messages**: Always prefix with `[CLAUDE] ` when making commits
- **After coding tasks**: Consider updating:
  - `CLAUDE.md` - Architecture and development patterns
  - `CHANGELOG.md` - Feature changes and bug fixes
  - `README.md` & `README.zh-CN.md` - New controls or features (keep both in sync)
