# Manta Controls Reference

All controls use the `https://github.com/avaloniaui` XML namespace — no extra xmlns needed.

---

## Avalonia Native Controls — Usage Rules

**Do not use Avalonia's built-in interactive controls.** Always use Manta's M\* equivalents
unless the control appears in the whitelist below.

### Whitelist — native Avalonia controls safe to use

**Styled by Manta** (use directly):
`TextBlock` · `SelectableTextBlock` · `Label` · `AccessText` ·
`Border` · `GridSplitter` · `Thumb` ·
`ToolTip` · `TransitioningContentControl` · `ThemeVariantScope`

**Layout panels** (no Manta equivalent — use freely):
`Grid` / `RowDefinition` / `ColumnDefinition` ·
`StackPanel` · `WrapPanel` · `DockPanel` · `UniformGrid` · `Canvas` · `Viewbox`

**Shapes** (vector graphics):
`Path` · `Arc` · `Ellipse` · `Line` · `Polygon` · `Polyline` · `Rectangle`

**Infrastructure** (inside control templates only):
`ContentPresenter` · `ItemsPresenter` · `Decorator` · `Popup` · `Image`

### Forbidden — never use these Avalonia controls

These are explicitly unsupported. Use Manta equivalents or avoid entirely:

`Calendar` · `CalendarButton` · `CalendarDatePicker` · `CalendarDayButton` · `CalendarItem` ·
`ContentPage` · `DataValidationErrors` · `DatePicker` · `DatePickerPresenter` ·
`GroupBox` · `NavigationPage` · `NotificationCard` · `PageNavigationHost` ·
`PipsPager` · `SplitView` · `TimePicker` · `TimePickerPresenter` · `WindowNotificationManager`

For any control **not** in the whitelist, use the `M` prefixed equivalent:
`Button` → `MButton`, `TextBox` → `MTextBox`, `ListBox` → `MListBox`, `Slider` → `MSlider`,
`TreeView` → `MTree`, `Menu` → `MMenu`, `CheckBox` → `MCheckBox`, etc.

---

## MPanel — Layout Container

The primary layout control. Replaces most uses of `StackPanel` and `WrapPanel`.

```xml
<!-- Vertical stack -->
<MPanel Mode="Vertical" Spacing="12" Margin="12">
    <TextBlock Text="Item 1" />
    <TextBlock Text="Item 2" />
</MPanel>

<!-- Horizontal stack -->
<MPanel Mode="Horizontal" Spacing="8">
    <MButton Content="A" />
    <MButton Content="B" />
</MPanel>

<!-- Wrap (items flow to next line) -->
<MPanel Mode="Wrap" Spacing="12">
    <MButton Content="Tag 1" />
    <MButton Content="Tag 2" />
</MPanel>

<!-- Column grid (fixed-width columns) -->
<MPanel Mode="Column" Column="3" ColumnSize="200">
    <!-- 3-column layout, 200px each -->
</MPanel>

<!-- Waterfall / masonry layout -->
<MPanel Mode="Waterfall" Column="3" Spacing="12">
    <!-- variable-height items -->
</MPanel>
```

**MPanelMode enum:**

| Mode | Description |
|------|-------------|
| `Vertical` | Stack vertically with spacing |
| `Horizontal` | Stack horizontally with spacing |
| `Wrap` | Flow to next line when full |
| `WrapFull` | Wrap with full-width expansion |
| `Column` | Fixed-width column grid |
| `Waterfall` | Masonry / Pinterest-style layout |
| `WaterfallBest` | Optimized waterfall positioning |
| `VerticalUniform` | Equal-height vertical items |
| `HorizontalUniform` | Equal-width horizontal items |

**Key properties:** `Spacing` (double), `Column` (int), `ColumnSize` (double), `ReverseOrder` (bool)

---

## MButton

```xml
<!-- Variants -->
<MButton Content="Primary" />
<MButton Variant="Secondary" Content="Secondary" />
<MButton Variant="Destructive" Content="Delete" />
<MButton Variant="Outline" Content="Outline" />
<MButton Variant="Ghost" Content="Ghost" />
<MButton Variant="Flat" Content="Flat" />

<!-- With command -->
<MButton Content="Save" Command="{Binding SaveCommand}" />

<!-- Disabled -->
<MButton Content="N/A" IsEnabled="False" />
```

**MButtonVariant:** `Default` · `Outline` · `Secondary` · `Ghost` · `Destructive` · `Flat`

---

## MCard

```xml
<MCard Width="300">
    <MPanel Mode="Vertical" Spacing="8" Margin="16">
        <TextBlock Text="Card Title" FontWeight="SemiBold" FontSize="16" />
        <TextBlock Text="Card description..." TextWrapping="Wrap" />
        <MButton Content="Action" />
    </MPanel>
</MCard>
```

MCard applies `MShadow MD` by default.

---

## MExpander

```xml
<MExpander Header="Section Title">
    <MPanel Mode="Vertical" Spacing="4">
        <TextBlock Text="Content line 1" />
        <TextBlock Text="Content line 2" />
    </MPanel>
</MExpander>

<MExpander Header="Details" IsExpanded="{Binding IsOpen}">
    <!-- ... -->
</MExpander>
```

---

## MAccordion

```xml
<MAccordion>
    <MAccordionItem Header="Section 1">
        <TextBlock Text="Content for section 1" />
    </MAccordionItem>
    <MAccordionItem Header="Section 2">
        <TextBlock Text="Content for section 2" />
    </MAccordionItem>
</MAccordion>
```

---

## MTextBox

```xml
<MTextBox Placeholder="Enter text..." Text="{Binding InputText}" />
<MTextBox Watermark="Search..." />
<MTextBox IsPasswordBox="True" Text="{Binding Password}" />
<MTextBox IsReadOnly="True" Text="{Binding DisplayText}" />

<!-- With inner content (icons, buttons) -->
<MTextBox Placeholder="Search..." Padding="0 8">
    <MTextBox.InnerLeftContent>
        <MIcon IconSize="16" Margin="8 0 0 0" VerticalAlignment="Center"
               IconSource="{x:Static Icons.Search}" />
    </MTextBox.InnerLeftContent>
    <MTextBox.InnerRightContent>
        <MButton Variant="Ghost" Margin="3 0" Padding="8 5"
                 VerticalAlignment="Center" Content="Clear" />
    </MTextBox.InnerRightContent>
</MTextBox>
```

**MTextBoxVariant:** `Default` · `NoStyle`

---

## MCheckBox

```xml
<MCheckBox Content="Enable feature" IsChecked="{Binding IsEnabled}" />

<!-- Variants -->
<MCheckBox Variant="Default" Content="Default" />
<MCheckBox Variant="Card" Content="Card style" />
```

**MCheckBoxVariant:** `Default` · `Card`

---

## MRadioButton

```xml
<MRadioButton Header="Option A" GroupName="MyGroup" IsChecked="{Binding IsA}" />
<MRadioButton Header="Option B" GroupName="MyGroup" />

<!-- Bordered variant (card-style) -->
<MRadioButton Variant="Bordered" Header="Premium" GroupName="plan" />
```

**MRadioButtonVariant:** `Default` · `Bordered`

---

## MToggleSwitch

```xml
<MToggleSwitch IsChecked="{Binding IsEnabled}" />
<MToggleSwitch IsChecked="{Binding DarkMode}" OnContent="On" OffContent="Off" />
```

---

## MToggleButton

```xml
<MToggleButton Content="Bold" IsChecked="{Binding IsBold}" />
```

**MToggleButtonVariant:** `Default` · `Outline`

---

## MListBox

```xml
<!-- Basic list -->
<MListBox ItemsSource="{Binding Items}" SelectedItem="{Binding SelectedItem}">
    <MListBox.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Name}" />
        </DataTemplate>
    </MListBox.ItemTemplate>
</MListBox>

<!-- Sidebar navigation style -->
<MListBox Variant="Sidebar" ItemsSource="{Binding NavItems}"
          SelectedItem="{Binding CurrentNav}">
    <MListBox.ItemTemplate>
        <DataTemplate>
            <MPanel Mode="Horizontal" Spacing="8">
                <MIcon IconSize="16" IconSource="{Binding Icon}"
                       Margin="0 -10" VerticalAlignment="Center" />
                <TextBlock Text="{Binding Name}" VerticalAlignment="Center" />
            </MPanel>
        </DataTemplate>
    </MListBox.ItemTemplate>
</MListBox>
```

**MListBoxVariant:** `Default` · `Sidebar` · `NoStyle`

---

## MIcon

```xml
<!-- Basic -->
<MIcon IconSize="24" IconSource="{x:Static Icons.Home}" />

<!-- Variants -->
<MIcon Variant="Default" IconSize="40" IconSource="{x:Static Icons.Settings}" />
<MIcon Variant="Secondary" IconSize="40" IconSource="{x:Static Icons.Info}" />
<MIcon Variant="Destructive" IconSize="40" IconSource="{x:Static Icons.Delete}" />

<!-- Custom colors -->
<MIcon IconSize="40"
       Background="{MBrush Primary, Opacity=0.1}"
       Foreground="{MBrush Primary}"
       IconSource="{x:Static Icons.Star}" />

<!-- Icon styles -->
<MIcon IconStyle="Default" IconSize="32" IconSource="..." />
<MIcon IconStyle="OnlyBorder" IconSize="32" IconSource="..." />
```

**MIconVariant:** `Default` · `Secondary` · `Destructive`

---

## MComboBox

```xml
<MComboBox ItemsSource="{Binding Options}" SelectedItem="{Binding Selected}"
           PlaceholderText="Select an option...">
    <MComboBox.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding}" />
        </DataTemplate>
    </MComboBox.ItemTemplate>
</MComboBox>
```

---

## MAlert (Control)

```xml
<MAlert Title="Heads up!" Message="Something important to know." />
<MAlert Variant="Destructive" Title="Error!" Message="Something went wrong." />
```

**MAlertVariant:** `Default` · `Destructive`

---

## MBadge

```xml
<MBadge Content="New" />
<MBadge Variant="Secondary" Content="Draft" />
<MBadge Variant="Destructive" Content="Error" />
<MBadge Variant="Outline" Content="v1.0" />
```

**MBadgeVariant:** `Default` · `Secondary` · `Destructive` · `Outline`

---

## MAvatar

```xml
<MAvatar Source="{Binding AvatarUrl}" Initials="AB" Size="40" />
```

---

## MImage

Async image control with built-in loading from multiple sources:

```xml
<MImage Source="https://example.com/photo.jpg" Width="200" Height="150" />
<MImage Source="avares://MyApp/Assets/logo.png" Width="100" />
<MImage Source="/path/to/local/file.png" />
```

`Source` accepts strings that are auto-converted to `IImageSource`:
- `https://...` → `WebImageSource` (cached via `ImageLoadService`)
- `avares://...` → `AvaloniaImageSource`
- File path → `LocalImageSource`

---

## MProgressRing

```xml
<MProgressRing IsIndeterminate="True" Width="32" Height="32" />
<MProgressRing Value="0.65" Width="48" Height="48" />
```

---

## MProgressBar

```xml
<MProgressBar Value="65" Minimum="0" Maximum="100" />
<MProgressBar IsIndeterminate="True" />
<MProgressBar Variant="Destructive" Value="30" />

<!-- Custom fill color -->
<MProgressBar BarBrush="{MBrush Primary}" Value="50" />
```

**MProgressVariant:** `Default` · `Destructive`

---

## MSlider

```xml
<MSlider Minimum="0" Maximum="100" Value="{Binding Volume}" />
<MSlider Orientation="Vertical" Minimum="0" Maximum="1" Value="{Binding Level}" />
```

---

## MNumericUpDown

```xml
<MNumericUpDown Value="{Binding Count}" Minimum="0" Maximum="100" Increment="1" />
```

---

## MAutoCompleteBox

```xml
<MAutoCompleteBox ItemsSource="{Binding Suggestions}"
                  Text="{Binding SearchText}"
                  Watermark="Type to search..." />
```

---

## MScrollViewer

```xml
<MScrollViewer MaxHeight="300">
    <MPanel Mode="Vertical" Spacing="8">
        <!-- Long content -->
    </MPanel>
</MScrollViewer>
```

---

## MTabStrip / MTabTitleBar

```xml
<!-- Tab strip for content switching -->
<MTabStrip ItemsSource="{Binding Tabs}" SelectedItem="{Binding ActiveTab}">
    <MTabStrip.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Title}" />
        </DataTemplate>
    </MTabStrip.ItemTemplate>
</MTabStrip>

<!-- Title bar tabs (main navigation) -->
<MTabTitleBar ItemsSource="{Binding RegionManager.Items}"
              SelectedItem="{Binding RegionManager.SelectedItem}">
    <MTabTitleBar.RightContent>
        <!-- Toolbar buttons go here -->
    </MTabTitleBar.RightContent>
    <MTabTitleBar.ItemTemplate>
        <DataTemplate>
            <MPanel Mode="Horizontal" Spacing="6">
                <MIcon IconSize="16" IconSource="{Binding Icon}"
                       Margin="0 -10" VerticalAlignment="Center" />
                <TextBlock Text="{Binding Header}" VerticalAlignment="Center" />
            </MPanel>
        </DataTemplate>
    </MTabTitleBar.ItemTemplate>
</MTabTitleBar>
```

**MTabVariant:** `Default` · `Outline`

---

## MTabControl

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

---

## MViewHost — Region Navigation

```xml
<!-- Displays the view for the current ViewModel -->
<MViewHost ViewModel="{Binding RegionManager.SelectedItem}" />

<!-- Direct ViewModel instantiation -->
<MViewHost>
    <MViewHost.ViewModel>
        <local:ChildView_Model />
    </MViewHost.ViewModel>
</MViewHost>
```

---

## MHyperlinkButton

```xml
<MHyperlinkButton NavigateUri="https://example.com" Content="Visit site" />
<MHyperlinkButton Variant="Subtle" Content="Learn more" Command="{Binding OpenDocsCommand}" />
```

---

## MSeparator

```xml
<!-- Horizontal divider (default) -->
<MSeparator />

<!-- Vertical divider -->
<MSeparator Orientation="Vertical" />
```

---

## MMenu / MContextMenu

```xml
<MMenu>
    <MMenuItem Header="File">
        <MMenuItem Header="New" Command="{Binding NewCommand}" />
        <MMenuItem Header="Open" Command="{Binding OpenCommand}" />
        <Separator />
        <MMenuItem Header="Exit" Command="{Binding ExitCommand}" />
    </MMenuItem>
    <MMenuItem Header="Edit">
        <MMenuItem Header="Copy" />
        <MMenuItem Header="Paste" />
    </MMenuItem>
</MMenu>

<!-- Context menu -->
<Border.ContextMenu>
    <MContextMenu>
        <MMenuItem Header="Copy" Command="{Binding CopyCommand}" />
        <MMenuItem Header="Delete" Command="{Binding DeleteCommand}" />
    </MContextMenu>
</Border.ContextMenu>
```

---

## MTree

```xml
<MTree ItemsSource="{Binding RootNodes}" Command="{Binding SelectNodeCommand}">
    <MTree.ItemTemplate>
        <TreeDataTemplate ItemsSource="{Binding Children}">
            <TextBlock Text="{Binding Name}" />
        </TreeDataTemplate>
    </MTree.ItemTemplate>
</MTree>
```

---

## MDropDownButton / MSplitButton

```xml
<MDropDownButton Content="Actions">
    <MDropDownButton.Flyout>
        <MMenuFlyout>
            <MMenuItem Header="Option A" />
            <MMenuItem Header="Option B" />
        </MMenuFlyout>
    </MDropDownButton.Flyout>
</MDropDownButton>

<MSplitButton Content="Save" Command="{Binding SaveCommand}">
    <MSplitButton.Flyout>
        <MMenuFlyout>
            <MMenuItem Header="Save As..." Command="{Binding SaveAsCommand}" />
        </MMenuFlyout>
    </MSplitButton.Flyout>
</MSplitButton>
```

---

## MItems

Generic items container that wraps each child in a styled `MItem`:

```xml
<MItems ItemsSource="{Binding Tags}">
    <MItems.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding}" />
        </DataTemplate>
    </MItems.ItemTemplate>
</MItems>
```

---

## MCommandPanel

Command palette / searchable command list:

```xml
<MCommandPanel ItemsSource="{Binding Commands}" SelectedItem="{Binding SelectedCommand}">
    <MCommandPanel.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Name}" />
        </DataTemplate>
    </MCommandPanel.ItemTemplate>
</MCommandPanel>
```

---

## MBreadcrumb

```xml
<MBreadcrumb ItemsSource="{Binding PathSegments}">
    <MBreadcrumb.ItemTemplate>
        <DataTemplate>
            <MBreadcrumbItem Content="{Binding Name}" Command="{Binding NavigateCommand}" />
        </DataTemplate>
    </MBreadcrumb.ItemTemplate>
</MBreadcrumb>
```

---

## MCarousel

```xml
<MCarousel ItemsSource="{Binding Slides}" SelectedIndex="{Binding CurrentSlide}">
    <MCarousel.ItemTemplate>
        <DataTemplate>
            <Image Source="{Binding ImageSource}" Stretch="UniformToFill" />
        </DataTemplate>
    </MCarousel.ItemTemplate>
</MCarousel>
```

---

## MCanvasGrid — Infinite Pan/Zoom Canvas

```xml
<MCanvasGrid Zoom="{Binding Zoom}"
             Offset="{Binding Offset}"
             ShowGrid="True"
             GridSize="20"
             MinZoom="0.1"
             MaxZoom="10">
    <Canvas>
        <Border Canvas.Left="100" Canvas.Top="100" Width="120" Height="80" />
    </Canvas>
</MCanvasGrid>
```

Key bindings: `Ctrl+Scroll` = zoom · `Scroll` = pan · `Cmd+1` = reset · `Cmd+2` = fit all

---

## MBackground + MDialogTemplate — Dialog Wrapper

For dialog views (see mvvm.md for full DialogModel workflow):

```xml
<MView x:Class="MyApp.Views.ConfirmView" ...>
  <MBackground MinWidth="{MSize S80}">
    <MDialogTemplate Header="Confirm Action"
                     CancelButton="Cancel"
                     ContinueButton="OK"
                     CancelButtonCommand="{Binding CancelAsyncCommand}"
                     ContinueButtonCommand="{Binding ContinueAsyncCommand}">
      <TextBlock Text="{Binding Message}" TextWrapping="Wrap" />
    </MDialogTemplate>
  </MBackground>
</MView>
```

---

## MAnimatedExpander

Smooth animated expand/collapse decorator:

```xml
<MAnimatedExpander IsExpanded="{Binding ShowDetails}">
    <MPanel Mode="Vertical" Spacing="8">
        <!-- collapsible content -->
    </MPanel>
</MAnimatedExpander>
```

---

## MBackdropBlur

```xml
<MBackdropBlur BlurRadius="20">
    <!-- content with blurred background -->
</MBackdropBlur>
```

---

## MClip

```xml
<MClip CornerRadius="12">
    <Image Source="{Binding Photo}" />
</MClip>
```

---

## MAspectRatio

```xml
<MAspectRatio Ratio="16:9">
    <Image Source="{Binding Thumbnail}" Stretch="UniformToFill" />
</MAspectRatio>
```

---

## MRingShape

```xml
<MRingShape StrokeThickness="4" Value="0.75" Width="48" Height="48" />
```

---

## MPixelGrid

```xml
<MPixelGrid PixelSize="10" Columns="16" Rows="16" />
```

---

## Attached Commands

### DoubleTappedCommand

```xml
<Border DoubleTappedCommand.Command="{Binding EditCommand}">
    <TextBlock Text="Double-tap to edit" />
</Border>
```

### DragDropCommand

```xml
<Border DragDropCommand.Command="{Binding DropCommand}">
    <TextBlock Text="Drop files here" />
</Border>
```

The command receives `DragEventArgs` as parameter:

```csharp
public CommandWarper<DragEventArgs> DropCommand => GetCommand<DragEventArgs>(e =>
{
    var file = e.DataTransfer.TryGetFile();
    if (file is not null)
        DroppedPath = file.Path.LocalPath;
});
```

`DragDropCommand` automatically calls `EnsureBackground()` and `SetAllowDrop(true)` on the target.

---

## MTitleBar

```xml
<MTitleBar>
    <MTitleBar.LeftContent>
        <MButton Variant="Ghost" Content="Menu" />
    </MTitleBar.LeftContent>
    <MTitleBar.RightContent>
        <MButton Variant="Ghost" Content="Settings" />
    </MTitleBar.RightContent>
</MTitleBar>
```

---

## Complete Variant Enum Summary

| Control | Enum | Values |
|---------|------|--------|
| `MButton` | `MButtonVariant` | Default, Outline, Secondary, Ghost, Destructive, Flat |
| `MTextBox` | `MTextBoxVariant` | Default, NoStyle |
| `MCheckBox` | `MCheckBoxVariant` | Default, Card |
| `MRadioButton` | `MRadioButtonVariant` | Default, Bordered |
| `MToggleButton` | `MToggleButtonVariant` | Default, Outline |
| `MListBox` | `MListBoxVariant` | Default, Sidebar, NoStyle |
| `MIcon` | `MIconVariant` | Default, Secondary, Destructive |
| `MAlert` | `MAlertVariant` | Default, Destructive |
| `MBadge` | `MBadgeVariant` | Default, Secondary, Destructive, Outline |
| `MProgressBar` | `MProgressVariant` | Default, Destructive |
| `MTabStrip`/`MTabControl` | `MTabVariant` | Default, Outline |
| `MDropDownButton` | `MDropDownButtonVariant` | Default, Outline, Secondary, Ghost, Destructive, Flat |
| `MSplitButton` | `MSplitButtonVariant` | Default, Outline, Secondary, Ghost, Destructive, Flat |
| `MToggleSplitButton` | `MToggleSplitButtonVariant` | Default, Outline, Secondary, Ghost, Destructive, Flat |
| `MHyperlinkButton` | `MHyperlinkButtonVariant` | Default, Subtle |
| `MRepeatButton` | `MRepeatButtonVariant` | Default, Outline, Secondary, Ghost, Destructive, Flat |
| `MNumericUpDown` | `MNumericUpDownVariant` | Default |
| `MCommandPanel` | `MCommandVariant` | Default |
