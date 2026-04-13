# UI Design Guide — Modern Minimal Layouts

Manta's design system maps to shadcn/ui & Tailwind conventions.
Use existing tokens; don't invent custom colors or magic numbers.

---

## Typography Scale

No dedicated type-scale control — compose directly with `TextBlock`:

| Role | FontSize | FontWeight | Color token |
|------|----------|------------|-------------|
| Display / Hero | 48–72 | `ExtraLight` / `Light` | `Foreground` |
| Page title / H1 | 24–32 | `SemiBold` | `Foreground` |
| Section heading / H2 | 18–20 | `SemiBold` | `Foreground` |
| Card title / H3 | 14–16 | `SemiBold` | `Foreground` |
| Body | 14 (default) | `Regular` | `Foreground` |
| Caption / Helper | 12 | `Regular` | `MutedForeground` |
| Label (above input) | 14 | `Regular` | `Foreground` |

```xml
<TextBlock Text="Dashboard" FontSize="28" FontWeight="SemiBold" />

<TextBlock Text="Recent activity" FontSize="12" FontWeight="SemiBold"
           Foreground="{MBrush MutedForeground}" />

<MPanel Mode="Vertical" Spacing="1">
    <TextBlock Text="Language" />
    <TextBlock Text="Change the display language" FontSize="12"
               Foreground="{MBrush MutedForeground}" />
</MPanel>
```

---

## Spacing Rhythm (4px base — Tailwind scale)

Prefer `{MThickness Sn}` tokens everywhere. Avoid raw numbers.

| Common use | Token | px |
|---|---|---|
| Tight gap between related items | `S1` | 4 |
| Icon ↔ label gap | `S1_5` | 6 |
| Intra-group gap | `S2` | 8 |
| Standard gap | `S3` | 12 |
| Section gap | `S4` | 16 |
| Large section gap | `S6` | 24 |
| Control padding (compact) | `S2, Vertical=S1` | 8 / 4 |
| Control padding (normal) | `S3, Vertical=S2` | 12 / 8 |
| Page inset | `S4` – `S6` | 16–24 |

---

## Shadows

`{MShadow <level>}` on `BoxShadow`. Use sparingly — only on floating surfaces.

| Level | Use |
|-------|-----|
| `None` | Flat / no elevation |
| `XS` | Subtle card lift |
| `SM` | Card default |
| `MD` | Dropdown, popover |
| `LG` | Modal, dialog |
| `XL` / `XL2` | Elevated overlays |

`MCard` applies `MShadow MD` by default.
For colored shadows: `{MShadow MD, BrushKey=Primary, BrushType=Dark3}`

---

## Semantic Color Usage

Map intent → token, matching shadcn/ui:

| Intent | Background | Text |
|--------|-----------|------|
| Page | `Background` | `Foreground` |
| Card / surface | `Card` | `CardForeground` |
| Subtle / muted area | `Muted` | `MutedForeground` |
| Primary action | `Primary` | `PrimaryForeground` |
| Secondary action | `Secondary` | `SecondaryForeground` |
| Danger action | `Destructive` | `DestructiveForeground` |
| Sidebar | `SidebarBackground` | `SidebarForeground` |
| Sidebar selected | `SidebarPrimary` | `SidebarPrimaryForeground` |

**Opacity compositing** — tinted surfaces without new tokens:

```xml
<!-- Subtle primary tint (hover state, highlight) -->
<Border Background="{MBrush Primary, Opacity=0.08}" />

<!-- Muted section header -->
<Border Background="{MBrush Muted}">
    <TextBlock Foreground="{MBrush MutedForeground}" FontWeight="SemiBold"
               Margin="{MThickness S2, Vertical=S1_5}" />
</Border>
```

---

## Layout Patterns

### 1. Two-Column Sidebar Layout (Settings / Nav)

```xml
<Grid ColumnDefinitions="200 *">
    <!-- Sidebar -->
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
            <MListBoxItem>
                <MPanel Mode="Horizontal" Spacing="8">
                    <MIcon IconSize="16" IconSource="{x:Static Icons.General}"
                           Margin="0 -10" VerticalAlignment="Center" />
                    <TextBlock Text="General" VerticalAlignment="Center" />
                </MPanel>
            </MListBoxItem>
        </MListBox>
    </Border>

    <!-- Content -->
    <Grid Grid.Column="1" RowDefinitions="Auto *">
        <Border Grid.Row="0" Height="39" BorderThickness="0 0 0 1"
                BorderBrush="{MBrush Border}">
            <TextBlock Text="General" FontWeight="SemiBold"
                       Margin="{MThickness S3}" VerticalAlignment="Center" />
        </Border>
        <MScrollViewer Grid.Row="1">
            <MPanel Mode="Vertical" Spacing="16" Margin="{MThickness S4}">
                <!-- content sections -->
            </MPanel>
        </MScrollViewer>
    </Grid>
</Grid>
```

### 2. Dashboard / Card Grid

```xml
<MScrollViewer>
    <MPanel Mode="Vertical" Spacing="24" Margin="{MThickness S6}">
        <TextBlock Text="Overview" FontSize="24" FontWeight="SemiBold" />

        <MPanel Mode="Column" ColumnSize="200" Spacing="16">
            <MCard>
                <MPanel Mode="Vertical" Spacing="6" Margin="{MThickness S4}">
                    <TextBlock Text="Total Users" FontSize="12"
                               Foreground="{MBrush MutedForeground}" />
                    <TextBlock Text="12,430" FontSize="28" FontWeight="SemiBold" />
                    <TextBlock Text="+8.2% this month" FontSize="12"
                               Foreground="{MBrush MutedForeground}" />
                </MPanel>
            </MCard>
        </MPanel>

        <MCard>
            <MPanel Mode="Vertical" Spacing="12" Margin="{MThickness S4}">
                <TextBlock Text="Recent Orders" FontWeight="SemiBold" />
                <MSeparator />
                <!-- table / list -->
            </MPanel>
        </MCard>
    </MPanel>
</MScrollViewer>
```

### 3. Form with Labeled Fields

```xml
<MPanel Mode="Vertical" Spacing="16" Margin="{MThickness S4}">
    <!-- Field -->
    <MPanel Mode="Vertical" Spacing="6">
        <TextBlock Text="Display name" />
        <MTextBox PlaceholderText="Enter your name..." Text="{Binding Name}" />
    </MPanel>

    <!-- Field with helper text -->
    <MPanel Mode="Vertical" Spacing="6">
        <TextBlock Text="API Base URL" />
        <TextBlock Text="Do NOT include /chat/completions in the URL" FontSize="12"
                   Foreground="{MBrush MutedForeground}" />
        <MTextBox PlaceholderText="https://api.openai.com/v1" Text="{Binding ApiUrl}" />
    </MPanel>

    <!-- Inline label + toggle -->
    <Grid ColumnDefinitions="* Auto" VerticalAlignment="Center">
        <MPanel Grid.Column="0" Mode="Vertical" Spacing="2">
            <TextBlock Text="Dark mode" />
            <TextBlock Text="Use system preference" FontSize="12"
                       Foreground="{MBrush MutedForeground}" />
        </MPanel>
        <MToggleSwitch Grid.Column="1" IsChecked="{Binding UseDarkMode}"
                       Margin="{MThickness S3, Filter=Left}" />
    </Grid>

    <!-- Actions -->
    <MPanel Mode="Horizontal" Spacing="8" HorizontalAlignment="Right">
        <MButton Variant="Ghost" Content="Cancel" Command="{Binding CancelCommand}" />
        <MButton Content="Save changes" Command="{Binding SaveCommand}" />
    </MPanel>
</MPanel>
```

### 4. Grouped Settings List (shadcn/ui "group" style)

Bordered container with dividers between rows:

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
                <Style Selector="^:nth-child(1)">
                    <Setter Property="BorderThickness" Value="0" />
                </Style>
            </Style>
        </MItems.Styles>

        <MItem>
            <Grid ColumnDefinitions="* Auto">
                <MPanel Grid.Column="0" Mode="Vertical" Spacing="2">
                    <TextBlock Text="Theme" />
                    <TextBlock Text="Light or dark appearance" FontSize="12"
                               Foreground="{MBrush MutedForeground}" />
                </MPanel>
                <MComboBox Grid.Column="1" Margin="{MThickness S3, Filter=Left}"
                           SelectedItem="{Binding Theme}" />
            </Grid>
        </MItem>

        <MItem>
            <Grid ColumnDefinitions="* Auto">
                <TextBlock Grid.Column="0" Text="Compact mode"
                           VerticalAlignment="Center" />
                <MToggleSwitch Grid.Column="1" IsChecked="{Binding CompactMode}"
                               Margin="{MThickness S3, Filter=Left}" />
            </Grid>
        </MItem>
    </MItems>
</MPanel>
```

### 5. Empty State

```xml
<MPanel Mode="Vertical" Spacing="12" HorizontalAlignment="Center"
        VerticalAlignment="Center" Margin="{MThickness S6}">
    <MIcon IconSize="48" IconSource="{x:Static Icons.Inbox}"
           Foreground="{MBrush MutedForeground}" HorizontalAlignment="Center" />
    <MPanel Mode="Vertical" Spacing="4" TextBlock.TextAlignment="Center">
        <TextBlock Text="No items yet" FontSize="16" FontWeight="SemiBold"
                   HorizontalAlignment="Center" />
        <TextBlock Text="Create your first item to get started."
                   Foreground="{MBrush MutedForeground}" HorizontalAlignment="Center" />
    </MPanel>
    <MButton Content="Create item" HorizontalAlignment="Center"
             Command="{Binding CreateCommand}" />
</MPanel>
```

---

## Icon Patterns

`MIcon` inside compact buttons requires a negative vertical margin to align with text baseline:

```xml
<!-- Icon-only button (compact) -->
<MButton Variant="Ghost" Padding="{MThickness S1_5}">
    <MIcon IconSize="16" IconSource="{x:Static Icons.Settings}" />
</MButton>

<!-- Icon + label in a button -->
<MButton Variant="Outline">
    <MPanel Mode="Horizontal" Spacing="6">
        <MIcon IconSize="16" IconSource="{x:Static Icons.Add}"
               Margin="0 -10" VerticalAlignment="Center" />
        <TextBlock Text="New item" VerticalAlignment="Center" />
    </MPanel>
</MButton>

<!-- Colored icon badge -->
<MIcon IconSize="40"
       Background="{MBrush Primary, Opacity=0.1}"
       Foreground="{MBrush Primary}"
       IconSource="{x:Static Icons.Star}" />
```

The `Margin="0 -10"` is required for 16–18px icons in normal-height buttons to cancel extra
icon container padding and achieve vertical centering.

---

## Design Principles

1. **Semantic tokens only** — never use literal hex values or `StaticResource` for colors. Compose
   all colors from `{MBrush …}` with `Opacity` / `Type` variants.

2. **Spacing rhythm** — use the 4px scale. Page inset `S4`–`S6`; sections `S4`–`S6`; related
   items `S2`–`S3`; tight pairs `S1`–`S1_5`.

3. **One accent color** — `Primary` is the single action color. `Destructive` only for
   irreversible operations. Avoid coloring decorative elements.

4. **Borders over backgrounds** — prefer a 1px `{MThickness Px}` + `{MBrush Border}` divider
   to separate regions, rather than different background fills. Works in both themes.

5. **Elevation sparingly** — `MCard` already adds `MShadow MD`. Don't stack shadows.
   `MShadow LG` only for overlaid surfaces (drawers, modals).

6. **Muted for metadata** — secondary text (timestamps, helpers, captions, counts) always uses
   `{MBrush MutedForeground}` at `FontSize="12"`.

7. **Actions right-aligned** — in dialogs and forms, primary + secondary action buttons sit on
   the right (`HorizontalAlignment="Right"`) with Ghost cancel left of Primary confirm.

8. **Ghost / Outline for secondary** — `Variant="Ghost"` for non-critical icon actions;
   `Variant="Outline"` for secondary explicit actions; default (Primary) only for the single
   primary CTA per surface.
