# Manta User Guide Skills

Skills for projects using the Manta UI toolkit as a dependency.

## Available Skills

### [project-structure.md](project-structure.md)
Recommended project structure and setup. Covers:
- Directory organization
- Naming conventions
- Application setup (App.axaml, Program.cs)
- Project file configuration

### [views.md](views.md)
MVVM architecture and ViewModel types. Covers:
- `Model` - Base class with property change notification
- `ViewModel` - For content views (MView)
- `WindowModel` - For window views (MWindow)
- `RegionModel` - For region-based navigation
- `ViewManager` - Navigation management

### [theme-resources.md](theme-resources.md)
Theme system and markup extensions. Covers:
- `{MBrush}` - Colors with variants and opacity
- `{MSize}` - Spacing values (4px grid)
- `{MThickness}` - Margin/padding
- `{MRadius}` - Corner radius
- `{MShadow}` - Box shadows

### [controls.md](controls.md)
Control library reference. Covers:
- Layout: `MPanel`, `MItems`, `MCard`
- Input: `MButton`, `MTextBox`, `MCheckBox`, `MComboBox`
- Display: `MListBox`, `MIcon`, `MBadge`, `MAlert`
- Container: `MExpander`, `MTabControl`, `MScrollViewer`
- Window: `MWindow`, `MTitleBar`, `MViewHost`

### [services.md](services.md)
Service locator pattern. Covers:
- Service registration
- Constructor injection
- Direct resolution
- Best practices

## Quick Start

### 1. Project Setup

```xml
<PackageReference Include="Manta" Version="*" />
```

### 2. App.axaml

```xml
<Application.Styles>
    <MantaTheme />
</Application.Styles>
```

### 3. Create a View

```csharp
// MyView_Model.cs
[View<MyView>]
sealed class MyView_Model : ViewModel
{
    public static MyView_Model Design => new();
}
```

```xml
<!-- MyView.axaml -->
<MView x:DataType="local:MyView_Model"
       Design.DataContext="{x:Static local:MyView_Model.Design}">
    <MPanel Mode="Vertical" Spacing="{MSize S3}">
        <MButton Content="Hello" />
    </MPanel>
</MView>
```

### 4. Open Window

```csharp
var window = ViewManager.OpenWindow<MainWindow_Model>();
```

## Common Patterns

### Button Row

```xml
<MPanel Mode="Horizontal" Spacing="{MSize S2}">
    <MButton Content="Save" />
    <MButton Variant="Secondary" Content="Cancel" />
</MPanel>
```

### Card with Shadow

```xml
<Border Background="{MBrush Card}"
        CornerRadius="{MRadius MD}"
        Padding="{MThickness S4}"
        BoxShadow="{MShadow SM}">
    <!-- Content -->
</Border>
```

### Settings Item

```xml
<Grid ColumnDefinitions="* Auto">
    <TextBlock Text="Dark Mode" />
    <MToggleSwitch IsChecked="{Binding DarkMode}" />
</Grid>
```
