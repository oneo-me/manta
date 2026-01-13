# Controls Guide

This guide covers the main Manta controls and their usage patterns.

## Control Naming

All Manta controls use the `M` prefix (e.g., `MButton`, `MPanel`, `MListBox`).

## Layout Controls

### MPanel

A flexible layout panel supporting multiple layout modes.

```xml
<!-- Vertical stack -->
<MPanel Mode="Vertical" Spacing="12">
    <TextBlock Text="Item 1" />
    <TextBlock Text="Item 2" />
</MPanel>

<!-- Horizontal stack -->
<MPanel Mode="Horizontal" Spacing="8">
    <MButton Content="Save" />
    <MButton Content="Cancel" />
</MPanel>

<!-- Wrap layout -->
<MPanel Mode="Wrap" Spacing="8">
    <MBadge Content="Tag 1" />
    <MBadge Content="Tag 2" />
    <MBadge Content="Tag 3" />
</MPanel>

<!-- Column layout (fixed columns) -->
<MPanel Mode="Column" Column="3" Spacing="12">
    <MCard />
    <MCard />
    <MCard />
</MPanel>

<!-- Waterfall layout -->
<MPanel Mode="Waterfall" Column="2" Spacing="12">
    <!-- Variable height items -->
</MPanel>

<!-- Uniform layout -->
<MPanel Mode="VerticalUniform" Spacing="4">
    <MButton Content="A" />
    <MButton Content="B" />
</MPanel>
```

**Layout Modes:**

| Mode | Description |
|------|-------------|
| `Vertical` | Stack items vertically |
| `Horizontal` | Stack items horizontally |
| `Wrap` | Wrap items to next line |
| `WrapFull` | Wrap with full-width last row |
| `Column` | Fixed column grid |
| `Waterfall` | Pinterest-style masonry |
| `WaterfallBest` | Waterfall with optimal placement |
| `VerticalUniform` | Vertical with equal heights |
| `HorizontalUniform` | Horizontal with equal widths |

### MItems

Container for grouped items with separator styling.

```xml
<MItems>
    <MItem>
        <TextBlock Text="Option 1" />
    </MItem>
    <MItem>
        <TextBlock Text="Option 2" />
    </MItem>
</MItems>
```

### MCard

Styled card container.

```xml
<MCard>
    <MPanel Mode="Vertical" Spacing="8">
        <TextBlock Text="Card Title" FontWeight="SemiBold" />
        <TextBlock Text="Card content" />
    </MPanel>
</MCard>
```

## Input Controls

### MButton

Button with multiple variants.

```xml
<!-- Default (primary filled) -->
<MButton Content="Submit" />

<!-- Variants -->
<MButton Variant="Default" Content="Default" />
<MButton Variant="Outline" Content="Outline" />
<MButton Variant="Secondary" Content="Secondary" />
<MButton Variant="Ghost" Content="Ghost" />
<MButton Variant="Destructive" Content="Delete" />
<MButton Variant="Link" Content="Learn more" />

<!-- With icon -->
<MButton Variant="Ghost" Padding="4">
    <MIcon IconSize="16" IconSource="{Binding MyIcon}" />
</MButton>
```

**Variants:**

| Variant | Use Case |
|---------|----------|
| `Default` | Primary actions |
| `Outline` | Secondary actions with border |
| `Secondary` | Less prominent actions |
| `Ghost` | Minimal/icon buttons |
| `Destructive` | Dangerous actions |
| `Link` | Navigation links |

### MTextBox

Enhanced text input.

```xml
<MTextBox Watermark="Enter your name..." />
<MTextBox Text="{Binding Email}" Watermark="Email" />
<MTextBox IsPasswordBox="True" Watermark="Password" />
```

### MCheckBox

Checkbox with optional header/content.

```xml
<MCheckBox IsChecked="{Binding AcceptTerms}"
           Header="Accept terms and conditions"
           Content="By checking this, you agree to our terms." />

<!-- Bordered variant -->
<MCheckBox Variant="Bordered"
           IsChecked="{Binding EnableNotifications}"
           Header="Enable notifications" />
```

### MRadioButton

Radio button with header/content.

```xml
<MPanel Mode="Vertical" Spacing="8">
    <MRadioButton GroupName="size" Header="Small" IsChecked="True" />
    <MRadioButton GroupName="size" Header="Medium" />
    <MRadioButton GroupName="size" Header="Large" />
</MPanel>
```

### MToggleSwitch

Toggle switch control.

```xml
<MToggleSwitch IsChecked="{Binding DarkMode}" />
```

### MComboBox

Dropdown selection.

```xml
<MComboBox SelectedIndex="0">
    <MComboBoxItem Content="Option 1" />
    <MComboBoxItem Content="Option 2" />
    <MComboBoxItem Content="Option 3" />
</MComboBox>
```

### MNumericUpDown

Numeric input with stepper.

```xml
<MNumericUpDown Value="{Binding Quantity}"
                Minimum="1"
                Maximum="100"
                Increment="1" />
```

## Display Controls

### MListBox

List selection control.

```xml
<!-- Default style -->
<MListBox>
    <MListBoxItem Content="Item 1" />
    <MListBoxItem Content="Item 2" />
</MListBox>

<!-- Sidebar style -->
<MListBox Vertical="Sidebar">
    <MListBoxItem>
        <MPanel Mode="Horizontal" Spacing="8">
            <MIcon IconSize="18" IconSource="{Binding Icon}" />
            <TextBlock Text="Settings" />
        </MPanel>
    </MListBoxItem>
</MListBox>
```

**Vertical Styles:**

| Style | Use Case |
|-------|----------|
| `Default` | Standard list |
| `Sidebar` | Navigation sidebar |
| `NoStyle` | Unstyled for custom styling |

### MIcon / MPathIcon

Icon display.

```xml
<!-- From IconSource -->
<MIcon IconSize="24" IconSource="{Binding MyIconSource}" />

<!-- Path icon with data -->
<MPathIcon Data="M12 2L2 12h10z" Size="16" />
<MPathIcon Data="M20 6 9 17l-5-5" UseStroke="True" Size="16" />
```

### MBadge

Badge/tag display.

```xml
<MBadge Content="New" />
<MBadge Content="3" />
```

### MAlert

Alert/notification display.

```xml
<MAlert Header="Information"
        Content="This is an informational message." />
```

### MProgressRing / MProgressBar

Progress indicators.

```xml
<MProgressRing IsIndeterminate="True" />
<MProgressRing Value="75" Maximum="100" />

<MProgressBar Value="{Binding Progress}" Maximum="100" />
<MProgressBar IsIndeterminate="True" />
```

### MBreadcrumb

Navigation breadcrumb.

```xml
<MBreadcrumb>
    <MBreadcrumbItem Content="Home" />
    <MBreadcrumbItem Content="Products" />
    <MBreadcrumbItem Content="Details" />
</MBreadcrumb>
```

## Container Controls

### MExpander

Collapsible content area.

```xml
<MExpander Header="Advanced Options" IsExpanded="False">
    <MPanel Mode="Vertical" Spacing="8">
        <!-- Expanded content -->
    </MPanel>
</MExpander>
```

### MAccordion

Multiple collapsible sections.

```xml
<MAccordion>
    <MAccordionItem Header="Section 1">
        <TextBlock Text="Content 1" />
    </MAccordionItem>
    <MAccordionItem Header="Section 2">
        <TextBlock Text="Content 2" />
    </MAccordionItem>
</MAccordion>
```

### MTabControl / MTabStrip

Tabbed content.

```xml
<MTabControl>
    <MTabItem Header="Tab 1">
        <TextBlock Text="Content 1" />
    </MTabItem>
    <MTabItem Header="Tab 2">
        <TextBlock Text="Content 2" />
    </MTabItem>
</MTabControl>
```

### MScrollViewer

Styled scroll container.

```xml
<MScrollViewer>
    <MPanel Mode="Vertical" Spacing="8">
        <!-- Scrollable content -->
    </MPanel>
</MScrollViewer>
```

## Window Controls

### MWindow

Application window.

```xml
<MWindow xmlns="https://github.com/avaloniaui"
         xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
         x:Class="YourApp.MainWindow">
    <!-- Window content -->
</MWindow>
```

### MTitleBar

Window title bar.

```xml
<MTitleBar Title="My Application">
    <MTitleBar.LeftContent>
        <MButton Variant="Ghost" Content="Menu" />
    </MTitleBar.LeftContent>
</MTitleBar>
```

### MViewHost

Host for ViewModel-based views.

```xml
<!-- With ViewManager -->
<MViewHost ViewManager="{Binding NavigationManager}" />

<!-- With specific type -->
<MViewHost ViewType="{x:Type local:MyView_Model}" />
```

## Utility Controls

### MClip

Clips content with rounded corners.

```xml
<MClip CornerRadius="8">
    <Image Source="photo.jpg" />
</MClip>
```

### MAspectRatio

Maintains aspect ratio.

```xml
<MAspectRatio Ratio="16:9">
    <Image Source="video-thumbnail.jpg" />
</MAspectRatio>
```

### MBackdropBlur

Background blur effect.

```xml
<MBackdropBlur BlurRadius="20">
    <Border Background="{MBrush Background, Opacity=0.8}">
        <!-- Content -->
    </Border>
</MBackdropBlur>
```

## Common Patterns

### Button with Icon and Text

```xml
<MButton Variant="Ghost">
    <MPanel Mode="Horizontal" Spacing="4">
        <MIcon IconSize="16" IconSource="{Binding SaveIcon}" />
        <TextBlock Text="Save" VerticalAlignment="Center" />
    </MPanel>
</MButton>
```

### Settings Row

```xml
<Grid ColumnDefinitions="* Auto">
    <MPanel Grid.Column="0" Mode="Vertical">
        <TextBlock Text="Dark Mode" />
        <TextBlock Text="Enable dark theme" Opacity="0.5" />
    </MPanel>
    <MToggleSwitch Grid.Column="1" IsChecked="{Binding DarkMode}" />
</Grid>
```

### Navigation Sidebar

```xml
<MListBox Vertical="Sidebar" SelectedIndex="0">
    <MListBoxItem>
        <MPanel Mode="Horizontal" Spacing="8">
            <MIcon IconSize="18" IconSource="{Binding HomeIcon}" />
            <TextBlock Text="Home" VerticalAlignment="Center" />
        </MPanel>
    </MListBoxItem>
    <MListBoxItem>
        <MPanel Mode="Horizontal" Spacing="8">
            <MIcon IconSize="18" IconSource="{Binding SettingsIcon}" />
            <TextBlock Text="Settings" VerticalAlignment="Center" />
        </MPanel>
    </MListBoxItem>
</MListBox>
```
