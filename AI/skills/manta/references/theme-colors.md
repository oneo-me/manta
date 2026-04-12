# Theme & Color System

## MBrush — Color Tokens

All colors are accessed via the `{MBrush}` markup extension using `MBrushKey` enum values.

### Token Reference

| Token | Purpose |
|-------|---------|
| `Background` | Page / window background |
| `Foreground` | Primary text |
| `Card` | Card surface |
| `CardForeground` | Text on cards |
| `Popover` / `PopoverForeground` | Dropdown / tooltip surface |
| `Primary` / `PrimaryForeground` | Brand color + contrast text |
| `Secondary` / `SecondaryForeground` | Secondary actions |
| `Muted` / `MutedForeground` | Disabled / subtle elements |
| `Accent` / `AccentForeground` | Highlight color |
| `Destructive` / `DestructiveForeground` | Danger / delete actions |
| `Border` | Borders and dividers |
| `Input` | Input field borders |
| `Ring` | Focus ring color |
| `Chart1` – `Chart5` | Chart colors |
| `Shadow` | Shadow color |
| `SidebarBackground` | Sidebar surface |
| `SidebarForeground` | Sidebar text |
| `SidebarPrimary` / `SidebarPrimaryForeground` | Sidebar brand color |
| `SidebarAccent` / `SidebarAccentForeground` | Sidebar highlight |
| `SidebarBorder` | Sidebar borders |
| `SidebarRing` | Sidebar focus ring |

### AXAML Usage

```xml
<Border Background="{MBrush Background}" />
<TextBlock Foreground="{MBrush Foreground}" />
<Border Background="{MBrush Primary}" />
<TextBlock Foreground="{MBrush PrimaryForeground}" />
<Border Background="{MBrush Destructive}" />

<!-- With opacity -->
<Border Background="{MBrush Primary, Opacity=0.12}" />
<Border Background="{MBrush Foreground, Opacity=0.04}" />

<!-- Lightness variants -->
<!-- Light1/2/3 = +0.1/0.2/0.3 lightness, Dark1/2/3 = -0.1/0.2/0.3 lightness -->
<Border Background="{MBrush Primary, Type=Light2, Opacity=0.3}" />
<Border Background="{MBrush Primary, Type=Dark1}" />
```

> **Rule:** Only use the built-in MBrushKey tokens. Do not define custom color resources —
> compose from existing tokens using `Opacity` and `Type` variants.

---

## MThickness — Spacing Scale

Tailwind-style 4px base unit. Use `{MThickness}` markup extension.

### Full Scale

| Token | px | Token | px | Token | px |
|-------|-----|-------|-----|-------|-----|
| `S0` | 0 | `S3` | 12 | `S8` | 32 |
| `Px` | 1 | `S3_5` | 14 | `S9` | 36 |
| `S0_5` | 2 | `S4` | 16 | `S10` | 40 |
| `S1` | 4 | `S5` | 20 | `S11` | 44 |
| `S1_5` | 6 | `S6` | 24 | `S12` | 48 |
| `S2` | 8 | `S7` | 28 | `S14` | 56 |
| `S2_5` | 10 | | | `S16`+ | 64+ |

### Usage Patterns

```xml
<!-- Uniform (all sides) -->
<Setter Property="Padding" Value="{MThickness S3}" />

<!-- Different horizontal / vertical -->
<Setter Property="Padding" Value="{MThickness S4, Vertical=S2}" />

<!-- Selective sides using Filter -->
<Setter Property="Padding" Value="{MThickness S2, Filter=Top}" />
<Setter Property="Padding" Value="{MThickness S3, Filter=Left|Right}" />

<!-- Border (1px all sides) -->
<Setter Property="BorderThickness" Value="{MThickness Px}" />

<!-- Border top only -->
<Setter Property="BorderThickness" Value="{MThickness Px, Filter=Top}" />
```

**Filter values:** `Left` · `Top` · `Right` · `Bottom` (combinable with `|`)

---

## MRadius — Corner Radius

| Token | px | Token | px |
|-------|-----|-------|-----|
| `None` | 0 | `LG` | 12 |
| `XS` | 3 | `XL` | 18 |
| `SM` | 6 | `XL2` | 24 |
| `MD` | 9 | `XL3` | 36 |
| | | `XL4` | 48 |
| | | `Full` | 9999 (pill) |

```xml
<Setter Property="CornerRadius" Value="{MRadius MD}" />
<Setter Property="CornerRadius" Value="{MRadius LG}" />
<Setter Property="CornerRadius" Value="{MRadius Full}" />  <!-- pill shape -->

<!-- Selective corners -->
<Setter Property="CornerRadius" Value="{MRadius LG, Filter=Top}" />
<Setter Property="CornerRadius" Value="{MRadius XL, Filter=LeftTop|RightTop}" />
```

**Filter values:** `LeftTop` · `RightTop` · `RightBottom` · `LeftBottom` · `Top` · `Bottom` ·
`Left` · `Right` (combinable with `|`)

---

## MShadow — Shadow Depth Scale

Use on `BoxShadow` property. Apply sparingly — only on floating surfaces.

| Level | Use case |
|-------|----------|
| `None` | Flat / no elevation |
| `XS` | Subtle card lift |
| `SM` | Card default |
| `MD` | Dropdown, popover |
| `LG` | Modal, dialog |
| `XL` / `XL2` | Elevated overlays |

```xml
<Setter Property="BoxShadow" Value="{MShadow MD}" />

<!-- Colored shadow -->
<Setter Property="BoxShadow" Value="{MShadow MD, BrushKey=Primary, BrushType=Dark3}" />
```

`MCard` applies `MShadow MD` by default.

---

## MSize — Sizing Scale

```xml
<Border Width="{MSize S80}" />
<MBackground MinWidth="{MSize S80}" />
```

---

## SmoothBrushTransition — Animated Color Changes

Always use `SmoothBrushTransition` instead of Avalonia's `BrushTransition` — it handles OKLch
interpolation and mixed brush types correctly.

```xml
<Setter Property="Transitions">
    <Transitions>
        <SmoothBrushTransition Property="Background" Duration="0:0:0.1" />
        <SmoothBrushTransition Property="BorderBrush" Duration="0:0:0.1" />
    </Transitions>
</Setter>
```

---

## OnTheme — Light/Dark Conditional Values

```xml
<!-- First value = Light mode, Dark= for dark mode -->
<Setter Property="Opacity" Value="{OnTheme 1.0, Dark=0.85}" />
<Setter Property="Background" Value="{OnTheme {MBrush Primary, Type=Light2, Opacity=0.3},
                                              Dark={MBrush Primary, Type=Light2, Opacity=0.45}}" />
```
