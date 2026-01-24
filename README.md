# Manta

[中文](README.zh-CN.md) | English

A comprehensive UI toolkit and MVVM framework built on Avalonia for cross-platform desktop applications.

## Features

- 40+ themeable custom controls with smooth animations
- Light/Dark theme support with OKLch color space
- Reactive MVVM infrastructure with attribute-based view registration
- Multi-language support with compile-time code generation
- Smooth brush transitions with premultiplied alpha blending

## Requirements

- .NET 10.0
- Avalonia 11.3.10

## Controls

### Layout & Container

| Control | Description |
|---------|-------------|
| MWindow | Custom window with title bar integration and theme support |
| MPanel | Flexible layout panel with horizontal/vertical modes |
| MCard | Container with border, shadow, and rounded corners |
| MExpander | Expandable container with animated content reveal |
| MAnimatedExpander | Height-animated container for smooth expand/collapse |
| MItems | Items control with customizable item templates |
| MView | View container for MVVM pattern |
| MViewHost | Host control for dynamic view switching |
| MClipContentControl | Content control with clipping support |

### Input

| Control | Description |
|---------|-------------|
| MButton | Button with multiple variants (Primary, Secondary, Outline, Ghost, Destructive) |
| MToggleButton | Toggle button with Outline and WithText variants |
| MCheckBox | Checkbox with Default and Bordered variants, custom header support |
| MTextBox | Text input with animated cursor and IME support |
| MComboBox | Dropdown selection control |
| MCommand | Command palette for searchable action menus |

### Display

| Control | Description |
|---------|-------------|
| MIcon | Icon display with size variants |
| MPathIcon | Vector path-based icon |
| MAvatar | User avatar with image or initials |
| MBadge | Status badge with color variants |
| MBreadcrumb | Navigation breadcrumb trail |
| MAlert | Alert message with severity levels |
| MProgressRing | Circular progress indicator |
| MRing | Decorative ring element |

### Data

| Control | Description |
|---------|-------------|
| MListBox | List with selection and virtualization |
| MChart | Data visualization chart |
| MCarousel | Carousel for cycling through content |
| MTabStrip | Tab navigation strip |
| MTabTitleBar | Browser-style tabbed title bar |

### Specialized

| Control | Description |
|---------|-------------|
| MTitleBar | Custom window title bar with drag support |
| MScrollViewer | Scroll viewer with styled scrollbars |
| MAspectRatio | Container maintaining aspect ratio |
| MPixelGrid | Pixel-aligned grid display |
| MBackgroundBlur | Background blur effect container |

### Layout Panels

| Panel | Description |
|-------|-------------|
| MWaterfallPanel | Waterfall/masonry layout |
| MWrapPanel | Wrapping flow layout |
| MAutoGrid | Auto-sizing grid |
| MUniformGrid | Uniform cell grid |

## Theming

### Color System

| Feature | Description |
|---------|-------------|
| OKLch Color Space | Perceptually uniform color interpolation |
| Theme Variants | Light and Dark theme support |
| Dynamic Resources | Runtime theme switching |
| SmoothBrushTransition | Animated color transitions between states |

### Theme Resources

| Resource | Description |
|----------|-------------|
| Primary | Primary brand color |
| Secondary | Secondary accent color |
| Background | Background color |
| Foreground | Text and icon color |
| BorderBrush | Border color |
| Destructive | Error/danger color |

## MVVM Framework

### Core Components

| Component | Description |
|-----------|-------------|
| ViewModel | Base class with reactive property support |
| WindowModel | Window-specific view model base |
| ViewService | View lifecycle and navigation management |
| @ViewAttribute | Attribute-based view registration |

### Services

| Feature | Description |
|---------|-------------|
| Service Locator | Dependency registration and resolution |
| Reactive Commands | Command infrastructure with async support |
| Property Change | Automatic change notification |

## Globalization

| Feature | Description |
|---------|-------------|
| YAML Language Files | Human-readable translation files |
| Source Generator | Compile-time language key generation |
| Markup Extensions | XAML binding for translations |

## Animation

| Feature | Description |
|---------|-------------|
| SmoothBrushTransition | Brush interpolation with premultiplied alpha |
| MAnimatedExpander | Height animation for auto-sized content |
| Animated Cursor | Smooth text cursor movement in MTextBox |

## License

MIT
