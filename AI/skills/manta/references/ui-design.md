# Manta UI Design System

This is the primary design reference for Manta UI work.
It combines the semantic token system, spacing and sizing primitives, layout patterns, and a
premium Apple-inspired visual recipe in one place.

Use this file first when designing interfaces or generating AXAML.

---

## 1. Core Design Model

Manta design starts from primitives, then composes into surfaces and flows.

- Colors come from `{MBrush ...}` tokens.
- Spacing comes from `{MThickness ...}`.
- Corners come from `{MRadius ...}`.
- Elevation comes from `{MShadow ...}`.
- Size constraints can come from `{MSize ...}`.
- Theme-specific values come from `{OnTheme ...}`.
- Animated brush changes use `SmoothBrushTransition`.

### Primary Rule

Build from Manta tokens first.
Use literal hex colors only when a page is intentionally targeting a branded marketing look that
cannot be expressed cleanly through existing semantic tokens.

---

## 2. Primitives

### 2.1 MBrush

| Token | Purpose |
|-------|---------|
| `Background` | Page or window background |
| `Foreground` | Primary text |
| `Card` / `CardForeground` | Card surface and card text |
| `Popover` / `PopoverForeground` | Dropdown, tooltip, transient surface |
| `Primary` / `PrimaryForeground` | Primary action color and contrast text |
| `Secondary` / `SecondaryForeground` | Secondary action surface |
| `Muted` / `MutedForeground` | Quiet surfaces, helper text, metadata |
| `Accent` / `AccentForeground` | Highlighted state |
| `Destructive` / `DestructiveForeground` | Dangerous actions |
| `Border` | Borders and dividers |
| `Input` | Input border treatment |
| `Ring` | Focus ring color |
| `Shadow` | Shadow brush |
| `SidebarBackground` / `SidebarForeground` | Sidebar surface and text |
| `SidebarPrimary` / `SidebarPrimaryForeground` | Sidebar selected state |
| `SidebarAccent` / `SidebarAccentForeground` | Sidebar highlighted state |
| `SidebarBorder` / `SidebarRing` | Sidebar divider and focus ring |
| `Chart1` - `Chart5` | Chart palette |

```xml
<Border Background="{MBrush Background}" />
<TextBlock Foreground="{MBrush Foreground}" />
<Border Background="{MBrush Primary}" />
<TextBlock Foreground="{MBrush PrimaryForeground}" />
<Border Background="{MBrush Primary, Opacity=0.12}" />
<Border Background="{MBrush Primary, Type=Light2, Opacity=0.25}" />
<Border Background="{MBrush Primary, Type=Dark1}" />
```

Rule:
Only use built-in `MBrushKey` tokens.
Compose variants through `Opacity` and `Type`.

### 2.2 MThickness

4px-based scale with dense small steps.

| Token | px | Token | px | Token | px |
|-------|----|-------|----|-------|----|
| `S0` | 0 | `S3` | 12 | `S8` | 32 |
| `Px` | 1 | `S3_5` | 14 | `S9` | 36 |
| `S0_5` | 2 | `S4` | 16 | `S10` | 40 |
| `S1` | 4 | `S5` | 20 | `S11` | 44 |
| `S1_5` | 6 | `S6` | 24 | `S12` | 48 |
| `S2` | 8 | `S7` | 28 | `S14` | 56 |
| `S2_5` | 10 | `S16+` | 64+ | | |

```xml
<Setter Property="Padding" Value="{MThickness S3}" />
<Setter Property="Padding" Value="{MThickness S4, Vertical=S2}" />
<Setter Property="Padding" Value="{MThickness S3, Filter=Left|Right}" />
<Setter Property="BorderThickness" Value="{MThickness Px, Filter=Top}" />
```

Thickness filter values:
`Left`, `Top`, `Right`, `Bottom`

### 2.3 MRadius

| Token | px |
|-------|----|
| `None` | 0 |
| `XS` | 3 |
| `SM` | 6 |
| `MD` | 9 |
| `LG` | 12 |
| `XL` | 18 |
| `XL2` | 24 |
| `XL3` | 36 |
| `XL4` | 48 |
| `Full` | 9999 |

```xml
<Setter Property="CornerRadius" Value="{MRadius LG}" />
<Setter Property="CornerRadius" Value="{MRadius Full}" />
<Setter Property="CornerRadius" Value="{MRadius LG, Filter=Top}" />
```

Radius filter values:
`LeftTop`, `RightTop`, `RightBottom`, `LeftBottom`, `Top`, `Bottom`, `Left`, `Right`

### 2.4 MShadow

| Level | Use |
|-------|-----|
| `None` | Flat surface |
| `XS` | Light card lift |
| `SM` | Default card |
| `MD` | Popover, dropdown |
| `LG` | Dialog, modal |
| `XL` / `XL2` | Elevated overlays |

```xml
<Setter Property="BoxShadow" Value="{MShadow MD}" />
<Setter Property="BoxShadow" Value="{MShadow MD, BrushKey=Primary, BrushType=Dark3}" />
```

Use `Border` with `BoxShadow="{MShadow MD}"` for elevated surfaces.

### 2.5 MSize, OnTheme, SmoothBrushTransition

```xml
<Border Width="{MSize S80}" />
<MBackground MinWidth="{MSize S80}" />

<Setter Property="Opacity" Value="{OnTheme 1.0, Dark=0.85}" />
<Setter Property="Background"
        Value="{OnTheme {MBrush Primary, Type=Light2, Opacity=0.3},
                        Dark={MBrush Primary, Type=Light2, Opacity=0.45}}" />

<Setter Property="Transitions">
    <Transitions>
        <SmoothBrushTransition Property="Background" Duration="0:0:0.1" />
        <SmoothBrushTransition Property="BorderBrush" Duration="0:0:0.1" />
    </Transitions>
</Setter>
```

---

## 3. Typography

Manta has no dedicated typography component.
Compose with `TextBlock` directly and keep the scale intentional.

### Default Product UI Scale

| Role | FontSize | FontWeight | Color |
|------|----------|------------|-------|
| Display / Hero | 48-72 | `ExtraLight` / `Light` | `Foreground` |
| Page title / H1 | 24-32 | `SemiBold` | `Foreground` |
| Section title / H2 | 18-20 | `SemiBold` | `Foreground` |
| Card title / H3 | 14-16 | `SemiBold` | `Foreground` |
| Body | 14 | `Regular` | `Foreground` |
| Label | 14 | `Regular` | `Foreground` |
| Caption / helper | 12 | `Regular` | `MutedForeground` |

```xml
<TextBlock Text="Overview" FontSize="28" FontWeight="SemiBold" />
<TextBlock Text="Change the display language"
           FontSize="12"
           Foreground="{MBrush MutedForeground}" />
```

### Apple-Inspired Typography Variant

Use this only for premium marketing or showcase pages.

| Role | Size | Weight | Line Height | Letter Spacing |
|------|------|--------|-------------|----------------|
| Hero display | 56 | `SemiBold` | 1.07 | -0.28px |
| Section heading | 40 | `SemiBold` | 1.10 | 0 |
| Tile heading | 28 | `Regular` | 1.14 | 0.196px |
| Card title | 21 | `Bold` | 1.19 | 0.231px |
| Body | 17 | `Regular` | 1.47 | -0.374px |
| Link / caption | 14 | `Regular` | 1.29-1.43 | -0.224px |
| Fine print | 12 | `Regular` | 1.33 | -0.12px |

Font family recommendation:

```xml
<Style Selector="TextBlock">
    <Setter Property="FontFamily"
            Value="SF Pro Text, SF Pro Display, Helvetica Neue, Helvetica, Arial, sans-serif" />
</Style>
```

### Typography Rules

- UI screens prefer restraint and consistency over decorative type hierarchy.
- Body text remains left-aligned in most app surfaces.
- Metadata and helper text use `MutedForeground` at `FontSize="12"`.
- Apple-style hero sections can center headlines and supporting copy.

---

## 4. Color and Surface Strategy

### Semantic Usage

| Intent | Background | Text |
|--------|-----------|------|
| Page | `Background` | `Foreground` |
| Card / standard surface | `Card` | `CardForeground` |
| Muted surface | `Muted` | `MutedForeground` |
| Primary action | `Primary` | `PrimaryForeground` |
| Secondary action | `Secondary` | `SecondaryForeground` |
| Destructive action | `Destructive` | `DestructiveForeground` |
| Sidebar | `SidebarBackground` | `SidebarForeground` |
| Sidebar selected | `SidebarPrimary` | `SidebarPrimaryForeground` |

### Standard Manta Rules

- Use semantic tokens only for product UI.
- Compose subtle highlights through `Opacity` and `Type`.
- Keep decorative color use sparse.
- Reserve `Destructive` for irreversible actions.
- Use `Ring` for focus visibility.

```xml
<Border Background="{MBrush Primary, Opacity=0.08}" />
<Border Background="{MBrush Muted}">
    <TextBlock Foreground="{MBrush MutedForeground}" />
</Border>
```

### Apple-Inspired Surface Recipe

This recipe sits on top of Manta primitives.
It is useful for landing pages, showcase views, and premium product moments.

| Visual role | Preferred value | Manta mapping |
|-------------|-----------------|---------------|
| Hero black section | `#000000` | page-local brush or dark `Background` |
| Light alternate section | `#f5f5f7` | light `Muted` / `Card` feel |
| Primary text on light | `#1d1d1f` | `Foreground` |
| Primary CTA blue | `#0071e3` | `Primary` |
| Link on light | `#0066cc` | darker `Primary` treatment |
| Link on dark | `#2997ff` | lighter `Primary` treatment |
| Dark card surface | `#272729` - `#2a2a2d` | dark `Card` or page-local surface |

### Apple-Inspired Rules

- Alternate black and light-gray sections for cinematic rhythm.
- Spend the accent budget on blue actions only.
- Let contrast do the structural work before adding borders.
- Keep borders rare and shadows soft.

### Apple-Inspired Extended Palette

| Role | Value | Usage |
|------|-------|-------|
| White | `#ffffff` | text on dark surfaces |
| Black 80 | `rgba(0, 0, 0, 0.8)` | secondary text on light backgrounds |
| Black 48 | `rgba(0, 0, 0, 0.48)` | tertiary text and subdued controls |
| Dark surface 1 | `#272729` | dark card surface |
| Dark surface 2 | `#262628` | subtle dark variation |
| Dark surface 3 | `#28282a` | elevated dark card |
| Dark surface 4 | `#2a2a2d` | emphasized dark card |
| Dark surface 5 | `#242426` | deep dark surface |
| Button active | `#ededf2` | pressed light state |
| Button light | `#fafafc` | filter and search controls |
| Overlay | `rgba(210, 210, 215, 0.64)` | media control scrim |
| White 32 | `rgba(255, 255, 255, 0.32)` | hover treatment in dark contexts |

---

## 5. Spacing, Corners, and Elevation

### Spacing Rhythm

| Use | Token | px |
|-----|-------|----|
| Tight pair | `S1` | 4 |
| Icon and label | `S1_5` | 6 |
| Intra-group | `S2` | 8 |
| Standard | `S3` | 12 |
| Section gap | `S4` | 16 |
| Large section gap | `S6` | 24 |
| Large page inset | `S8`+ | 32+ |

### Corner Guidance

- Forms and standard cards: `MD` or `LG`
- Dialogs and floating panels: `LG` to `XL`
- Rounded feature surfaces: `XL` or `XL2`
- Pill CTAs and chips: `Full`

### Elevation Guidance

- Default app layout: flat or `XS`
- Elevated surfaces: use `Border` with `MShadow MD`
- Menus and popovers: `MD`
- Dialogs: `LG`
- Showcase cards: local custom shadow is acceptable if visual fidelity matters

### Apple-Inspired Physicality

- Rectangular surfaces stay around 5px to 12px radius.
- Pill CTAs use capsule corners.
- Use a single wide soft shadow like `rgba(0, 0, 0, 0.22) 3px 5px 30px 0px` for hero cards.
- Large spacing outside, tight spacing inside.

---

## 6. Component Rules

### Buttons

- Primary action: default `MButton`
- Secondary explicit action: `Variant="Outline"`
- Secondary quiet action: `Variant="Ghost"`
- Forms and dialogs place actions on the right

```xml
<MPanel Mode="Horizontal" Spacing="8" HorizontalAlignment="Right">
    <MButton Variant="Ghost" Content="Cancel" />
    <MButton Content="Save changes" />
</MPanel>
```

### Inputs and Form Rows

- Labels sit above fields in most settings and forms.
- Helper text uses `MutedForeground` at 12px.
- Toggles and selectors align on the trailing edge.

### Icons

Compact buttons often need icon baseline compensation.

```xml
<MButton Variant="Outline">
    <MPanel Mode="Horizontal" Spacing="6">
        <MIcon IconSize="16"
               IconSource="{x:Static Icons.Add}"
               Margin="0 -10"
               VerticalAlignment="Center" />
        <TextBlock Text="New item" VerticalAlignment="Center" />
    </MPanel>
</MButton>
```

### Apple-Inspired CTA Variant

```xml
<MPanel Mode="Horizontal" Spacing="12" HorizontalAlignment="Center">
    <MButton Variant="Outline"
             Content="Learn more"
             CornerRadius="{MRadius Full}"
             Padding="18,8" />
    <MButton Content="Buy"
             CornerRadius="{MRadius Full}"
             Padding="18,8" />
</MPanel>
```

### Apple-Inspired Control Variants

- Filter or search buttons use a light surface, subtle dark text, and about 11px corner radius.
- Media controls use circular 44x44 targets with soft translucent fills.
- Standard rectangular Apple-style controls stay in the 5px to 12px radius range.

### Navigation and Structural Chrome

- Standard app navigation uses dividers and semantic surfaces.
- Showcase navigation can use a translucent dark glass look.
- Compact nav target height is about 48px.

---

## 7. Layout Patterns

### Two-Column Sidebar Layout

```xml
<Grid ColumnDefinitions="200 *">
    <Border Grid.Column="0" BorderThickness="0 0 1 0" BorderBrush="{MBrush Border}">
        <MListBox Variant="Sidebar">
            <MListBox.Styles>
                <Style Selector="MListBox[Variant=Sidebar]">
                    <Setter Property="Padding" Value="{MThickness S1_5}" />
                </Style>
                <Style Selector="MListBoxItem[Variant=Sidebar]">
                    <Setter Property="Padding" Value="{MThickness S3, Vertical=S1_5}" />
                </Style>
            </MListBox.Styles>
        </MListBox>
    </Border>

    <Grid Grid.Column="1" RowDefinitions="Auto *">
        <Border Grid.Row="0" Height="39" BorderThickness="0 0 0 1"
                BorderBrush="{MBrush Border}">
            <TextBlock Text="General"
                       FontWeight="SemiBold"
                       Margin="{MThickness S3}"
                       VerticalAlignment="Center" />
        </Border>
        <MScrollViewer Grid.Row="1">
            <MPanel Mode="Vertical" Spacing="16" Margin="{MThickness S4}" />
        </MScrollViewer>
    </Grid>
</Grid>
```

### Dashboard / Card Grid

```xml
<MScrollViewer>
    <MPanel Mode="Vertical" Spacing="24" Margin="{MThickness S6}">
        <TextBlock Text="Overview" FontSize="24" FontWeight="SemiBold" />

        <MPanel Mode="Column" ColumnSize="200" Spacing="16">
            <Border Background="{MBrush Background}"
                    BorderBrush="{MBrush Border}"
                    BorderThickness="{MThickness Px}"
                    CornerRadius="{MRadius XL}"
                    Padding="{MThickness S4}"
                    BoxShadow="{MShadow MD}">
                <MPanel Mode="Vertical" Spacing="6">
                    <TextBlock Text="Total Users"
                               FontSize="12"
                               Foreground="{MBrush MutedForeground}" />
                    <TextBlock Text="12,430" FontSize="28" FontWeight="SemiBold" />
                </MPanel>
            </Border>
        </MPanel>
    </MPanel>
</MScrollViewer>
```

### Form with Labeled Fields

```xml
<MPanel Mode="Vertical" Spacing="16" Margin="{MThickness S4}">
    <MPanel Mode="Vertical" Spacing="6">
        <TextBlock Text="Display name" />
        <MTextBox PlaceholderText="Enter your name..." Text="{Binding Name}" />
    </MPanel>

    <MPanel Mode="Vertical" Spacing="6">
        <TextBlock Text="API Base URL" />
        <TextBlock Text="Use the API root URL"
                   FontSize="12"
                   Foreground="{MBrush MutedForeground}" />
        <MTextBox Text="{Binding ApiUrl}" />
    </MPanel>
</MPanel>
```

### Grouped Settings List

```xml
<MPanel Mode="Vertical" Spacing="6">
    <TextBlock Text="Appearance" FontWeight="SemiBold" />
    <MItems>
        <MItems.Styles>
            <Style Selector="MItems">
                <Setter Property="BorderThickness" Value="1" />
                <Setter Property="CornerRadius" Value="{MRadius LG}" />
                <Setter Property="Background" Value="{MBrush Border, Opacity=0.15}" />
            </Style>
            <Style Selector="MItem">
                <Setter Property="BorderBrush" Value="{MBrush Border}" />
                <Setter Property="BorderThickness" Value="{MThickness Px, Filter=Top}" />
                <Setter Property="Padding" Value="{MThickness S3, Vertical=S2_5}" />
            </Style>
        </MItems.Styles>
    </MItems>
</MPanel>
```

### Empty State

```xml
<MPanel Mode="Vertical"
        Spacing="12"
        HorizontalAlignment="Center"
        VerticalAlignment="Center"
        Margin="{MThickness S6}">
    <MIcon IconSize="48"
           IconSource="{x:Static Icons.Inbox}"
           Foreground="{MBrush MutedForeground}"
           HorizontalAlignment="Center" />
    <MPanel Mode="Vertical" Spacing="4" TextBlock.TextAlignment="Center">
        <TextBlock Text="No items yet" FontSize="16" FontWeight="SemiBold" />
        <TextBlock Text="Create your first item to get started."
                   Foreground="{MBrush MutedForeground}" />
    </MPanel>
    <MButton Content="Create item" HorizontalAlignment="Center" />
</MPanel>
```

### Apple-Inspired Product Hero

```xml
<Border Background="#000000">
    <MPanel Mode="Vertical"
            Spacing="20"
            HorizontalAlignment="Center"
            Margin="32,64">
        <TextBlock Text="MacBook Pro"
                   Foreground="White"
                   FontSize="56"
                   FontWeight="SemiBold"
                   HorizontalAlignment="Center" />
        <TextBlock Text="Mind-blowing. Head-turning."
                   Foreground="White"
                   FontSize="21"
                   HorizontalAlignment="Center" />
        <MPanel Mode="Horizontal" Spacing="12" HorizontalAlignment="Center">
            <MButton Variant="Outline"
                     Content="Learn more"
                     CornerRadius="{MRadius Full}"
                     Padding="18,8" />
            <MButton Content="Buy"
                     CornerRadius="{MRadius Full}"
                     Padding="18,8" />
        </MPanel>
    </MPanel>
</Border>
```

---

## 8. Responsive Behavior

| Breakpoint | Width | Guidance |
|------------|-------|----------|
| Small mobile | `<360` | single column, tighter insets |
| Mobile | `360-480` | single column, stack crowded actions |
| Large mobile | `480-640` | more image width, maintain simple structure |
| Tablet | `640-1024` | 2-column layouts become viable |
| Desktop | `1024+` | full layout, centered max-width content |

### Responsive Rules

- Preserve information hierarchy before preserving exact geometry.
- Collapse columns before shrinking text below comfortable sizes.
- Keep primary actions obvious and reachable.
- Marketing layouts preserve section rhythm and hero impact across breakpoints.

### Touch Targets

- Primary actions target roughly 44px height.
- Compact nav rows target about 48px height.
- Circular media controls stay at least 44x44.

---

## 9. Design Principles

1. Use semantic tokens for app UI by default.
2. Use the spacing scale consistently across layout, padding, and borders.
3. Let one primary action dominate each surface.
4. Prefer dividers and spacing for structure before adding extra fills.
5. Keep shadow use sparse and intentional.
6. Put metadata in `MutedForeground`.
7. Keep secondary actions visually quieter than the primary path.
8. Choose a visual mode explicitly: product UI mode or showcase marketing mode.

---

## 10. Agent Guidance

### When to use this guide

- Building app screens, settings, forms, dashboards, empty states, dialogs
- Choosing tokens, spacing, corner radius, or shadow levels
- Translating a premium visual direction into Manta AXAML
- Designing Apple-like showcase pages while staying grounded in Manta primitives

### Prompt snippets

- Product UI:
  `Build a settings screen using Manta semantic tokens, S4 page insets, LG surface radius, MutedForeground helper text, and right-aligned primary actions.`
- Dashboard:
  `Build a dashboard with elevated Border surfaces, SemiBold headings, muted metadata, 16-24px spacing rhythm, and semantic Primary actions.`
- Apple-inspired:
  `Build a premium product hero with alternating black and light-gray sections, SF Pro-style typography, capsule CTAs, restrained blue action color, and minimal chrome.`
