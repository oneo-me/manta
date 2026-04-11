# Manta

[中文](README.zh-CN.md) | English

A comprehensive UI toolkit and MVVM framework built on Avalonia for cross-platform desktop applications.

## Features

- 60+ themeable custom controls with smooth animations
- Light/Dark theme support with OKLch color space
- Reactive MVVM infrastructure with attribute-based view registration
- Tab/region navigation with `RegionManager`
- Built-in modal dialog system with `DialogModel`
- Multi-language support with compile-time code generation
- `SmoothBrushTransition` for correct OKLch brush interpolation

## Requirements

- .NET 10.0
- Avalonia 11.3.10

## Controls

### Layout & Container

| Control           | Description                                              |
|-------------------|----------------------------------------------------------|
| MWindow           | Custom window with title bar integration and theme support |
| MPanel            | Flexible layout panel (Vertical/Horizontal/Wrap/Column/Waterfall) |
| MCard             | Container with border, shadow, and rounded corners       |
| MExpander         | Expandable container with animated content reveal        |
| MAnimatedExpander | Height-animated container for smooth expand/collapse     |
| MAccordion        | Accordion control with multiple collapsible sections     |
| MItems            | Items control with customizable item templates           |
| MView             | View container for MVVM pattern                          |
| MViewHost         | Host control for dynamic view switching                  |
| MClip             | Content control with clipping support                    |
| MBackground       | Environment background for dialog and popup surfaces     |
| MDialogTemplate   | Unified dialog layout (header + cancel/continue buttons) |

### Input

| Control           | Description                                                              |
|-------------------|--------------------------------------------------------------------------|
| MButton           | Button with variants (Primary, Secondary, Outline, Ghost, Flat, Destructive) |
| MToggleButton     | Toggle button with Outline and WithText variants                         |
| MToggleSwitch     | Toggle switch control                                                    |
| MCheckBox         | Checkbox with Default and Card variants, custom header support           |
| MRadioButton      | Radio button with Default and Bordered variants                          |
| MTextBox          | Text input with animated cursor and IME support                          |
| MMaskedTextBox    | Text input with input mask support                                       |
| MAutoCompleteBox  | Text input with auto-completion dropdown                                 |
| MNumericUpDown    | Numeric input with increment/decrement                                   |
| MComboBox         | Dropdown selection control                                               |
| MSlider           | Horizontal and vertical slider                                           |
| MCommandPanel     | Command palette for searchable action menus                              |
| MDropDownButton   | Button with attached dropdown menu                                       |
| MSplitButton      | Button with split primary action and dropdown                            |
| MToggleSplitButton | Toggle split button                                                     |
| MRepeatButton     | Button that fires repeatedly while held                                  |
| MButtonSpinner    | Input with increment/decrement spinner buttons                           |
| MFlyout           | Flyout popup container                                                   |

### Display

| Control       | Description                               |
|---------------|-------------------------------------------|
| MIcon         | Icon display with size and style variants |
| MPathIcon     | Vector path-based icon                    |
| MImage        | Image display control                     |
| MAvatar       | User avatar with image or initials        |
| MBadge        | Status badge with color variants          |
| MBreadcrumb   | Navigation breadcrumb trail               |
| MAlert        | Alert/notification with severity levels   |
| MProgressRing | Circular progress indicator               |
| MProgressBar  | Horizontal progress bar                   |
| MRingShape    | Decorative ring / arc shape               |
| MSeparator    | Horizontal or vertical divider            |
| MHyperlinkButton | Hyperlink-style button                 |

### Data & Navigation

| Control      | Description                            |
|--------------|----------------------------------------|
| MListBox     | List with selection and virtualization |
| MTree        | Hierarchical tree view                 |
| MCarousel    | Carousel for cycling through content   |
| MTabStrip    | Tab navigation strip                   |
| MTabControl  | Full tab control with content panels   |
| MTabTitleBar | Browser-style tabbed title bar         |
| MMenu        | Application menu bar                   |

### Specialized

| Control        | Description                                  |
|----------------|----------------------------------------------|
| MTitleBar      | Custom window title bar with drag support    |
| MScrollViewer  | Scroll viewer with styled scrollbars         |
| MAspectRatio   | Container maintaining aspect ratio           |
| MPixelGrid     | Pixel-aligned grid display                   |
| MBackdropBlur  | Background blur effect container             |
| MCanvasGrid    | Infinite pan/zoom canvas with grid overlay   |
| MToolTip       | Styled tooltip                               |

### Layout Panels

`MPanel` wraps all layout modes through a single `Mode` property:

| Mode             | Description                              |
|------------------|------------------------------------------|
| `Vertical`       | Vertical stack                           |
| `Horizontal`     | Horizontal stack                         |
| `Wrap`           | Wrapping flow layout                     |
| `WrapFull`       | Wrap that stretches items to full width  |
| `Column`         | Fixed-column grid                        |
| `Waterfall`      | Waterfall/masonry layout                 |
| `WaterfallBest`  | Auto-balanced waterfall                  |
| `VerticalUniform` | Uniform-height vertical stack           |
| `HorizontalUniform` | Uniform-width horizontal stack       |

## Theming

### Color System

| Feature               | Description                               |
|-----------------------|-------------------------------------------|
| OKLch Color Space     | Perceptually uniform color interpolation  |
| Light / Dark themes   | Full light and dark theme support         |
| Dynamic Resources     | Runtime theme switching via `MApp.SwitchTheme()` |
| SmoothBrushTransition | OKLch brush interpolation between states  |
| OnTheme               | Conditional values per theme variant      |

### Design Tokens

| Token class   | Description                              |
|---------------|------------------------------------------|
| `MBrush`      | Semantic color tokens (30+ named keys)   |
| `MThickness`  | Spacing scale — 4 px base (S0–S96)       |
| `MRadius`     | Corner radius scale (None → Full)        |
| `MShadow`     | Shadow depth scale (None → XL2)          |
| `MSize`       | Sizing scale                             |
| `MRing`       | Focus ring style                         |

Key `MBrush` tokens: `Background` · `Foreground` · `Card` · `Primary` · `Secondary` · `Muted` ·
`Accent` · `Destructive` · `Border` · `Input` · `Ring` · `Sidebar*` · `Chart1`–`Chart5`

## MVVM Framework

### Class Hierarchy

```
Model
  └── CommandModel
      └── ViewModel
          ├── RegionModel      ← tab/region view model
          └── WindowModel
              ├── DialogModel  ← modal dialog with Result
              └── TrayWindowModel
```

### Core Components

| Component        | Description                                          |
|------------------|------------------------------------------------------|
| `ViewModel`      | Base class with reactive properties and lifecycle    |
| `WindowModel`    | Top-level window view model                          |
| `DialogModel`    | Modal dialog with built-in Cancel / Continue commands |
| `TrayWindowModel`| System-tray popup (auto-closes on focus loss)        |
| `RegionModel`    | Tab/region view model with `Close()` support         |
| `RegionManager<T>`| Tab list with observable selection                  |
| `ViewManager`    | Single-slot view switcher                            |
| `ViewTemplate<T>`| `IDataTemplate` that creates views from data objects |
| `ViewContext<T>` | Parent-to-child context propagation through the tree |
| `@ViewAttribute` | Attribute-based ViewModel ↔ View binding             |
| `CommandWarper`  | Cached command with `IsExecuting` and `OnChanged()`  |

### Service Locator

```csharp
// Register
Service.Register(new MyService());

// Consume (field initializer)
readonly MyService _svc = Service.GetService<MyService>();
```

### App Entry Point

```csharp
sealed class App : MApp
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    protected override void OnInitialize(out MAppOptions options)
    {
        Service.Register(new ConfigService());

        options = new()
        {
            MainModel = () => new MainView_Model(),
            Exit = () => configService.Save()
        };
    }
}
```

## Globalization

| Feature             | Description                          |
|---------------------|--------------------------------------|
| YAML Language Files | Flat key-value translation files     |
| Source Generator    | Compile-time `LangKey` constants     |
| `{Lang}` extension  | Reactive XAML translation binding    |
| `{LangEmoji}`       | Flag emoji for a `LangType` value    |
| `{LangName}`        | Native language name                 |
| 40+ `LangType` enum | Built-in language codes              |

## Platform APIs

Extension methods on `ViewModel` / `WindowModel` — thread-safe, no boilerplate:

| Method                    | Description              |
|---------------------------|--------------------------|
| `OpenFilePickerAsync`     | Open file picker dialog  |
| `OpenFolderPickerAsync`   | Open folder picker dialog |
| `SaveFilePickerAsync`     | Save file picker dialog  |
| `GetClipboardAsync`       | Get window clipboard     |

## License

MIT
