# Theme Resources Guide

This guide covers the Manta theme system and its markup extensions for consistent styling.

## Overview

Manta provides a set of XAML markup extensions for accessing theme values:

| Extension | Purpose | Example |
|-----------|---------|---------|
| `{MBrush}` | Colors and brushes | `{MBrush Primary}` |
| `{MSize}` | Spacing sizes | `{MSize S4}` |
| `{MThickness}` | Margin/padding | `{MThickness S3}` |
| `{MRadius}` | Corner radius | `{MRadius MD}` |
| `{MShadow}` | Box shadows | `{MShadow SM}` |

## MBrush - Colors

### Available Colors

| Key | Purpose |
|-----|---------|
| `Background` | Main background |
| `Foreground` | Main text color |
| `Card` | Card surfaces |
| `CardForeground` | Text on cards |
| `Popover` | Popup surfaces |
| `PopoverForeground` | Text on popups |
| `Primary` | Primary actions (purple) |
| `PrimaryForeground` | Text on primary |
| `Secondary` | Secondary elements |
| `SecondaryForeground` | Text on secondary |
| `Muted` | Muted backgrounds |
| `MutedForeground` | Muted text |
| `Accent` | Accent elements |
| `AccentForeground` | Text on accent |
| `Destructive` | Destructive actions (red) |
| `DestructiveForeground` | Text on destructive |
| `Border` | Border color |
| `Input` | Input backgrounds |
| `Ring` | Focus rings |
| `Chart1` - `Chart5` | Chart colors |
| `SidebarBackground` | Sidebar background |
| `SidebarForeground` | Sidebar text |
| `SidebarPrimary` | Sidebar primary |
| `SidebarAccent` | Sidebar accent |
| `SidebarBorder` | Sidebar borders |
| `Shadow` | Shadow color |

### Basic Usage

```xml
<Border Background="{MBrush Primary}" />
<TextBlock Foreground="{MBrush Foreground}" />
<Border BorderBrush="{MBrush Border}" />
```

### With Opacity

```xml
<Border Background="{MBrush Foreground, Opacity=0.1}" />
<Border Background="{MBrush Primary, Opacity=0.5}" />
```

### With Type (Lightness Adjustment)

```xml
<!-- Lighter variants -->
<Border Background="{MBrush Primary, Type=Light1}" />  <!-- +10% lightness -->
<Border Background="{MBrush Primary, Type=Light2}" />  <!-- +20% lightness -->
<Border Background="{MBrush Primary, Type=Light3}" />  <!-- +30% lightness -->

<!-- Darker variants -->
<Border Background="{MBrush Primary, Type=Dark1}" />   <!-- -10% lightness -->
<Border Background="{MBrush Primary, Type=Dark2}" />   <!-- -20% lightness -->
<Border Background="{MBrush Primary, Type=Dark3}" />   <!-- -30% lightness -->
```

## MSize - Spacing

Based on a 4px grid system with spacing multiplier.

### Available Sizes

| Variant | Calculation | Pixels |
|---------|-------------|--------|
| `S0` | 0 | 0 |
| `Px` | 1 | 1 |
| `S0_5` | 4 × 0.5 | 2 |
| `S1` | 4 × 1 | 4 |
| `S1_5` | 4 × 1.5 | 6 |
| `S2` | 4 × 2 | 8 |
| `S2_5` | 4 × 2.5 | 10 |
| `S3` | 4 × 3 | 12 |
| `S3_5` | 4 × 3.5 | 14 |
| `S4` | 4 × 4 | 16 |
| `S5` | 4 × 5 | 20 |
| `S6` | 4 × 6 | 24 |
| `S7` | 4 × 7 | 28 |
| `S8` | 4 × 8 | 32 |
| `S9` | 4 × 9 | 36 |
| `S10` | 4 × 10 | 40 |
| `S11` | 4 × 11 | 44 |
| `S12` | 4 × 12 | 48 |
| `S14` | 4 × 14 | 56 |
| `S16` | 4 × 16 | 64 |
| `S20` | 4 × 20 | 80 |
| `S24` | 4 × 24 | 96 |
| ... | ... | ... |

### Usage

```xml
<Border Width="{MSize S8}" Height="{MSize S8}" />
<MPanel Spacing="{MSize S3}" />
```

### With Offset

```xml
<!-- Add offset -->
<Border Width="{MSize S4, Offset=S2}" />        <!-- S4 + S2 -->

<!-- Subtract offset -->
<Border Width="{MSize S4, OffsetNegative=S1}" /> <!-- S4 - S1 -->
```

## MThickness - Margin/Padding

Creates `Thickness` values using the spacing system.

### Basic Usage

```xml
<Border Padding="{MThickness S3}" />           <!-- All sides: 12px -->
<Border Margin="{MThickness S2}" />
```

### With Vertical Override

```xml
<!-- Horizontal=S3 (12px), Vertical=S2 (8px) -->
<Border Padding="{MThickness S3, Vertical=S2}" />
```

### With Filter (Specific Sides)

```xml
<!-- Only left and right -->
<Border Padding="{MThickness S3, Filter='Left|Right'}" />

<!-- Only top -->
<Border Margin="{MThickness S2, Filter='Top'}" />

<!-- Bottom and right -->
<Border Margin="{MThickness S4, Filter='Bottom|Right'}" />
```

Available filters: `Left`, `Top`, `Right`, `Bottom`, `All`

### With Reverse (Negative Values)

```xml
<Border Margin="{MThickness S2, Reverse=True}" />  <!-- -8px all sides -->
```

## MRadius - Corner Radius

### Available Variants

| Variant | Calculation | Pixels |
|---------|-------------|--------|
| `None` | 0 | 0 |
| `XS` | 12 × 0.25 | 3 |
| `SM` | 12 × 0.5 | 6 |
| `MD` | 12 × 0.75 | 9 |
| `LG` | 12 × 1.0 | 12 |
| `XL` | 12 × 1.5 | 18 |
| `XL2` | 12 × 2.0 | 24 |
| `XL3` | 12 × 3.0 | 36 |
| `XL4` | 12 × 4.0 | 48 |
| `Full` | 9999 | Circle |

### Basic Usage

```xml
<Border CornerRadius="{MRadius MD}" />
<Border CornerRadius="{MRadius Full}" />  <!-- Pill shape -->
```

### With Filter (Specific Corners)

```xml
<!-- Top corners only -->
<Border CornerRadius="{MRadius MD, Filter='Top'}" />

<!-- Left corners only -->
<Border CornerRadius="{MRadius LG, Filter='Left'}" />

<!-- Single corner -->
<Border CornerRadius="{MRadius SM, Filter='LeftTop'}" />
```

Available filters: `LeftTop`, `RightTop`, `LeftBottom`, `RightBottom`, `Left`, `Right`, `Top`, `Bottom`, `All`

### With Offset

```xml
<Border CornerRadius="{MRadius SM, OffsetNegative=Px}" />
```

## MShadow - Box Shadows

### Available Variants

| Variant | Description |
|---------|-------------|
| `None` | No shadow |
| `XS` | Extra small shadow |
| `SM` | Small shadow |
| `MD` | Medium shadow |
| `LG` | Large shadow |
| `XL` | Extra large shadow |
| `XL2` | Double extra large shadow |

### Basic Usage

```xml
<Border BoxShadow="{MShadow SM}" />
<Border BoxShadow="{MShadow MD}" />
```

### Inset Shadow

```xml
<Border BoxShadow="{MShadow SM, ValueType=InsetShadow}" />
```

### Shadow Margin (for overflow clipping)

```xml
<!-- Positive margin to contain shadow -->
<Border Margin="{MShadow MD, ValueType=Margin}" />

<!-- Negative margin to pull shadow inward -->
<Border Margin="{MShadow MD, ValueType=MarginNegative}" />
```

### Custom Brush

```xml
<Border BoxShadow="{MShadow MD, BrushKey=Primary, BrushOpacity=0.3}" />
```

## Example: Complete Card

```xml
<Border Background="{MBrush Card}"
        BorderBrush="{MBrush Border}"
        BorderThickness="{MThickness Px}"
        CornerRadius="{MRadius MD}"
        Padding="{MThickness S4}"
        BoxShadow="{MShadow SM}">
    <MPanel Mode="Vertical" Spacing="{MSize S3}">
        <TextBlock Text="Card Title"
                   Foreground="{MBrush CardForeground}"
                   FontWeight="SemiBold" />
        <TextBlock Text="Card content goes here."
                   Foreground="{MBrush MutedForeground}" />
        <MButton Content="Action" Variant="Primary" />
    </MPanel>
</Border>
```

## Best Practices

1. **Use Semantic Colors** - Prefer `Primary`, `Secondary`, `Destructive` over raw colors

2. **Consistent Spacing** - Always use `MSize` and `MThickness` for consistent rhythm

3. **Border Thickness** - Use `{MThickness Px}` for 1px borders

4. **Accessibility** - Ensure adequate contrast between foreground and background colors

5. **Dark Mode** - All `MBrush` colors automatically adapt to light/dark themes
