# Theme & Color System

## MBrush — Color Tokens

Available color tokens (MBrushKey enum):

| Token | Purpose |
|-------|---------|
| `Background` | Page/window background |
| `Foreground` | Primary text |
| `Card` | Card surface |
| `CardForeground` | Text on cards |
| `Popover` / `PopoverForeground` | Dropdown/tooltip surface |
| `Primary` / `PrimaryForeground` | Brand color + contrast text |
| `Secondary` / `SecondaryForeground` | Secondary actions |
| `Muted` / `MutedForeground` | Disabled/subtle elements |
| `Accent` / `AccentForeground` | Highlight color |
| `Destructive` / `DestructiveForeground` | Danger/delete actions |
| `Border` | Borders and dividers |
| `Input` | Input field borders |
| `Ring` | Focus ring color |
| `Chart1`–`Chart5` | Chart colors |
| `Shadow` | Shadow color |
| `SidebarBackground` | Sidebar surface |
| `SidebarForeground` | Sidebar text |
| `SidebarPrimary` / `SidebarPrimaryForeground` | Sidebar brand color |
| `SidebarAccent` / `SidebarAccentForeground` | Sidebar highlight |
| `SidebarBorder` | Sidebar borders |
| `SidebarRing` | Sidebar focus ring |

**AXAML Usage:**
```xml
<Border Background="{MBrush Background}" />
<TextBlock Foreground="{MBrush Foreground}" />
<Border Background="{MBrush Primary}" />
<TextBlock Foreground="{MBrush PrimaryForeground}" />
<Border Background="{MBrush Destructive}" />

<!-- With opacity -->
<Border Background="{MBrush Primary, Opacity=0.12}" />
<Border Background="{MBrush Foreground, Opacity=0.04}" />

<!-- Lightness variants (Light1/2/3 = +0.1/0.2/0.3, Dark1/2/3 = -0.1/0.2/0.3) -->
<Border Background="{MBrush Primary, Type=Light2, Opacity=0.3}" />
<Border Background="{MBrush Primary, Type=Dark1}" />
```

## MThickness — Spacing Scale

Full scale (Tailwind-style, 4px unit):

| Token | px | Token | px | Token | px |
|-------|-----|-------|-----|-------|-----|
| `S0` | 0 | `S3` | 12 | `S8` | 32 |
| `Px` | 1 | `S3_5` | 14 | `S9` | 36 |
| `S0_5` | 2 | `S4` | 16 | `S10` | 40 |
| `S1` | 4 | `S5` | 20 | `S11` | 44 |
| `S1_5` | 6 | `S6` | 24 | `S12` | 48 |
| `S2` | 8 | `S7` | 28 | `S14`+ | 56+ |
| `S2_5` | 10 | | | | |

```xml
<!-- Uniform -->
<Setter Property="Padding" Value="{MThickness S3}" />

<!-- Different H/V -->
<Setter Property="Padding" Value="{MThickness S4, Vertical=S2}" />

<!-- Selective sides: Left, Top, Right, Bottom (combinable with |) -->
<Setter Property="Padding" Value="{MThickness S2, Filter=Top}" />
<Setter Property="Padding" Value="{MThickness S3, Filter=Left|Right}" />

<!-- Border (1px) -->
<Setter Property="BorderThickness" Value="{MThickness Px}" />
```

## MRadius — Corner Radius

Scale: `None`(0) · `XS`(3) · `SM`(6) · `MD`(9) · `LG`(12) · `XL`(18) · `XL2`(24) · `XL3`(36) · `XL4`(48) · `Full`(9999)

```xml
<Setter Property="CornerRadius" Value="{MRadius MD}" />
<Setter Property="CornerRadius" Value="{MRadius LG}" />
<Setter Property="CornerRadius" Value="{MRadius Full}" />  <!-- pill shape -->

<!-- Selective corners: TopLeft, TopRight, BottomRight, BottomLeft -->
<Setter Property="CornerRadius" Value="{MRadius LG, Filter=Top}" />
<Setter Property="CornerRadius" Value="{MRadius XL, Filter=LeftTop|RightTop}" />
```

## SmoothBrushTransition — Animated Color Changes

```xml
<Setter Property="Transitions">
    <Transitions>
        <SmoothBrushTransition Property="Background" Duration="0:0:0.1" />
        <SmoothBrushTransition Property="BorderBrush" Duration="0:0:0.1" />
    </Transitions>
</Setter>
```

Use this instead of `BrushTransition` — it handles OKLch interpolation and mixed brush types correctly.

## Light/Dark Theme Conditional Values

```xml
<!-- OnTheme: first value = Light, Dark= second value -->
<Setter Property="Opacity" Value="{OnTheme 1.0, Dark=0.85}" />
<Setter Property="Background" Value="{OnTheme {MBrush Primary, Type=Light2, Opacity=0.3},
                                              Dark={MBrush Primary, Type=Light2, Opacity=0.45}}" />
```

> **Rule:** Only use the built-in MBrushKey tokens listed above. Do not define custom color resources — always compose from existing tokens using `Opacity` and `Type` variants.
