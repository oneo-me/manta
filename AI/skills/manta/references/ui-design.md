# Manta UI Design System

This is the primary design reference for Manta UI work.
It combines the semantic token system, spacing and sizing primitives, layout patterns, and a
soft warm visual recipe in one place.

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

### Default Visual Personality

Manta now defaults to a soft, warm, calm product feel:

- light backgrounds should feel like parchment or ivory, not cold white
- dark surfaces should stay warm charcoal, not blue-black
- primary emphasis should be restrained, earthy, and low-saturation
- typography should feel quiet and practical by default, with editorial moments used sparingly
- structure should come from spacing, alignment, and subtle tonal grouping before containers
- depth should stay minimal: prefer flat surfaces, dividers, and whisper-soft shadows
- avoid overly modular "card everywhere" layouts unless the information truly benefits from them

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
| `XS` | Light surface lift |
| `SM` | Subtle grouped surface |
| `MD` | Popover, dropdown |
| `LG` | Dialog, modal |
| `XL` / `XL2` | Elevated overlays |

```xml
<Setter Property="BoxShadow" Value="{MShadow MD}" />
<Setter Property="BoxShadow" Value="{MShadow MD, BrushKey=Primary, BrushType=Dark3}" />
```

Use `Border` with `BoxShadow="{MShadow MD}"` only when a surface needs clear separation.

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

### Editorial Typography Variant

Use this for premium marketing, showcase pages, rich empty states, and hero sections.
Default application screens should stay closer to the product UI scale.

| Role | Size | Weight | Line Height | Letter Spacing |
|------|------|--------|-------------|----------------|
| Hero display | 56-64 | `Medium` | 1.08-1.12 | 0 |
| Section heading | 40-52 | `Medium` | 1.12-1.20 | 0 |
| Tile heading | 28-32 | `Medium` | 1.14-1.22 | 0 |
| Card title | 20-24 | `Medium` | 1.18-1.24 | 0 |
| Intro body | 18-20 | `Regular` | 1.55-1.60 | 0 |
| Body | 16-17 | `Regular` | 1.55-1.60 | 0 |
| Link / caption | 14 | `Regular` | 1.35-1.45 | 0.1px |
| Fine print | 12 | `Regular` | 1.33-1.50 | 0.12px |

Font family recommendation:

```xml
<Style Selector="Border.EditorialScope TextBlock.h1,
                 Border.EditorialScope TextBlock.h2,
                 Border.EditorialScope TextBlock.h3">
    <Setter Property="FontFamily"
            Value="Georgia, Cambria, Times New Roman, serif" />
</Style>

<Style Selector="TextBlock">
    <Setter Property="FontFamily"
            Value="Segoe UI, Noto Sans, Helvetica Neue, Arial, sans-serif" />
</Style>
```

### Typography Rules

- UI screens prefer restraint and consistency over decorative type hierarchy.
- Body text remains left-aligned in most app surfaces.
- Metadata and helper text use `MutedForeground` at `FontSize="12"`.
- Premium editorial sections can center headlines and supporting copy.
- Prefer serif only for major headings or quote-like passages, not for dense form UI.
- For everyday desktop views, avoid dramatic display sizing and keep hierarchy compact and readable.

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

### Soft Warm Surface Recipe

This recipe sits on top of Manta primitives.
It is useful for everyday app views, settings screens, workspaces, and polished dashboards that
should feel calm instead of card-heavy.

| Visual role | Preferred value | Manta mapping |
|-------------|-----------------|---------------|
| Page background | `#f5f4ed` | `Background` |
| Elevated light surface | `#faf9f5` | `Card` / `Popover` |
| Soft quiet surface | `#f0eee6` | `Muted` |
| Strong primary action | `#c96442` | `Primary` |
| Alternate accent | `#d97757` | `Accent` |
| Primary text on light | `#141413` | `Foreground` |
| Secondary text on light | `#5e5d59` | `MutedForeground` |
| Deep dark section | `#141413` | dark `Background` |
| Dark elevated surface | `#30302e` | dark `Card` / `Popover` |

### Soft Warm Rules

- Keep page backgrounds open and continuous; only introduce containers when grouping materially helps.
- Spend the accent budget on primary actions and small highlights, not on every section.
- Keep neutral grays warm; avoid blue-gray drift.
- Let spacing, typography, and subtle tone shifts do most of the work before adding decoration.
- Prefer thin dividers, muted section fills, and very soft shadows over obvious cards.
- When a card is necessary, make it broad and quiet rather than dense and repeated.

### Soft Warm Extended Palette

| Role | Value | Usage |
|------|-------|-------|
| Ivory | `#faf9f5` | text and surfaces on dark sections |
| Warm sand | `#e8e6dc` | button surface, active light state |
| Border cream | `#f0eee6` | light borders and separators |
| Warm charcoal | `#4d4c48` | button text on warm light surfaces |
| Olive gray | `#5e5d59` | secondary text |
| Stone gray | `#87867f` | tertiary text |
| Dark warm | `#3d3d3a` | emphasized dark-neutral text |
| Warm silver | `#b0aea5` | secondary text on dark sections |
| Muted green | `#6f8065` | restrained secondary accent |
| Focus blue | `#3898ec` | accessibility-only focus ring |

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

- Forms and grouped sections: `MD` or `LG`
- Dialogs and floating panels: `LG` to `XL`
- Rounded feature surfaces: `LG` or `XL`
- Pill CTAs and chips: `Full`

### Elevation Guidance

- Default app layout: flat or `XS`
- Elevated surfaces: use `Border` with `MShadow MD` sparingly
- Menus and popovers: `MD`
- Dialogs: `LG`
- Avoid stacking multiple elevated panels in the same viewport unless hierarchy demands it

### Calm Physicality

- Standard surfaces live around `MD` to `LG` radius with occasional `XL` showcase moments.
- Primary buttons and inputs can use `MD` or `LG`; `Full` should be intentional, not automatic.
- Use whisper-soft warm shadows instead of dramatic cold drop shadows.
- Favor larger outer spacing, quieter inner spacing, and stable visual rhythm.

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

### Editorial CTA Variant

```xml
<MPanel Mode="Horizontal" Spacing="12">
    <MButton Variant="Ghost"
             Content="Read more"
             CornerRadius="{MRadius LG}"
             Padding="16,8"
             Background="{MBrush Secondary}" />
    <MButton Content="Start writing"
             CornerRadius="{MRadius LG}"
             Padding="16,8" />
</MPanel>
```

### Editorial Control Variants

- Filter or search buttons use warm sand surfaces and warm charcoal text.
- Embedded media or visual callouts can use `XL` or `XL2` corners.
- Standard controls stay in the 6px to 12px radius range unless a showcase moment needs softer geometry.

### Navigation and Structural Chrome

- Standard app navigation uses dividers and semantic surfaces.
- Showcase navigation can use a translucent dark glass look.
- Compact nav target height is about 48px.
- Toolbars and section headers should usually blend into the page with separators instead of cards.

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

### Dashboard / Soft Section Grid

```xml
<MScrollViewer>
    <MPanel Mode="Vertical" Spacing="24" Margin="{MThickness S6}">
        <TextBlock Text="Overview" FontSize="24" FontWeight="SemiBold" />

        <MPanel Mode="Column" ColumnSize="200" Spacing="16">
            <Border Background="{MBrush Muted, Opacity=0.45}"
                    BorderBrush="{MBrush Border, Opacity=0.45}"
                    BorderThickness="{MThickness Px}"
                    CornerRadius="{MRadius LG}"
                    Padding="{MThickness S4}">
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
                <Setter Property="BorderThickness" Value="{MThickness Px}" />
                <Setter Property="BorderBrush" Value="{MBrush Border, Opacity=0.55}" />
                <Setter Property="CornerRadius" Value="{MRadius LG}" />
                <Setter Property="Background" Value="{MBrush Muted, Opacity=0.35}" />
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

### Editorial Product Hero

```xml
<Border Classes="EditorialScope" Background="{MBrush Background}">
    <MPanel Mode="Vertical"
            Spacing="20"
            HorizontalAlignment="Center"
            Margin="32,64">
        <TextBlock Classes="h1"
                   Text="Thoughtful tools need thoughtful surfaces"
                   Foreground="{MBrush Foreground}"
                   FontSize="60"
                   FontWeight="Medium"
                   HorizontalAlignment="Center" />
        <TextBlock Text="Use warm neutrals, open spacing, and gentle grouping to make the interface feel composed instead of generic."
                   Foreground="{MBrush MutedForeground}"
                   FontSize="18"
                   TextAlignment="Center"
                   MaxWidth="720"
                   HorizontalAlignment="Center" />
        <MButton Content="Explore the system"
                 CornerRadius="{MRadius LG}"
                 HorizontalAlignment="Center" />
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
8. Avoid repeating small cards when a grouped section or list can communicate the structure more calmly.
9. Choose a visual mode explicitly: calm product UI mode or warm editorial showcase mode.

---

## 10. Agent Guidance

### When to use this guide

- Building app screens, settings, forms, dashboards, empty states, dialogs
- Choosing tokens, spacing, corner radius, or shadow levels
- Translating a premium visual direction into Manta AXAML
- Designing warm editorial showcase pages while staying grounded in Manta primitives

### Prompt snippets

- Product UI:
  `Build a settings screen using Manta semantic tokens, S4 page insets, LG surface radius at most, muted grouped sections, minimal borders, and right-aligned primary actions.`
- Dashboard:
  `Build a dashboard with soft grouped sections, restrained elevation, muted metadata, 16-24px spacing rhythm, and semantic Primary actions.`
- Editorial showcase:
  `Build a premium product hero with parchment and dark chapter sections, serif-led headings, warm neutral surfaces, terracotta CTA buttons, and soft ring-based depth.`
