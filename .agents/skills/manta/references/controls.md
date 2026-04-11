# Manta Controls Reference

All controls are in the `https://github.com/avaloniaui` namespace — no extra xmlns needed.

## Native Avalonia Controls — Usage Rules

**Do not use Avalonia built-in controls.** Always use Manta's M\* equivalents unless the control
appears in the whitelist below.

### Whitelist — native Avalonia controls that may be used

**Styled by Manta** (safe to use directly):
`TextBlock` · `SelectableTextBlock` · `Label` · `AccessText` ·
`Border` · `GridSplitter` · `Thumb` ·
`ToolTip` · `TransitioningContentControl` · `ThemeVariantScope`

**Layout panels** (no Manta equivalent — use as needed):
`Grid` / `RowDefinition` / `ColumnDefinition` ·
`StackPanel` · `WrapPanel` · `DockPanel` · `UniformGrid` · `Canvas` · `Viewbox`

**Shapes** (vector graphics):
`Path` · `Arc` · `Ellipse` · `Line` · `Polygon` · `Polyline` · `Rectangle`

**Infrastructure** (typically used inside control templates only):
`ContentPresenter` · `ItemsPresenter` · `Decorator` · `Popup` · `Image`

All other controls must use Manta equivalents (`MButton`, `MTextBox`, `MPanel`, `MMenu`,
`MTree`, `MSlider`, `MRadioButton`, `MToggleSwitch`, `MNumericUpDown`, etc.).

## MPanel — Layout Container

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

<!-- Column grid -->
<MPanel Mode="Column" Column="3" ColumnSize="200">
    <!-- 3-column layout, 200px each -->
</MPanel>

<!-- Waterfall (Pinterest-style) -->
<MPanel Mode="Waterfall" Column="3" Spacing="12">
    <!-- variable-height items -->
</MPanel>
```

Modes: `Vertical` · `Horizontal` · `Wrap` · `WrapFull` · `Column` · `Waterfall` · `WaterfallBest` · `VerticalUniform` · `HorizontalUniform`

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

## MCard

```xml
<MCard Width="300">
    <MPanel Mode="Vertical" Spacing="8" Margin="16">
        <TextBlock Text="Card Title" FontWeight="SemiBold" FontSize="16" />
        <TextBlock Text="Card description text..." TextWrapping="Wrap" />
        <MButton Content="Action" />
    </MPanel>
</MCard>
```

## MExpander

```xml
<MExpander Header="Section Title">
    <MPanel Mode="Vertical" Spacing="4">
        <TextBlock Text="Content line 1" />
        <TextBlock Text="Content line 2" />
    </MPanel>
</MExpander>

<!-- Bound -->
<MExpander Header="Details" IsExpanded="{Binding IsOpen}">
    <!-- ... -->
</MExpander>
```

## MTextBox

```xml
<MTextBox Placeholder="Enter text..." Text="{Binding InputText}" />
<MTextBox Watermark="Search..." />
<MTextBox IsPasswordBox="True" Text="{Binding Password}" />
<MTextBox IsReadOnly="True" Text="{Binding DisplayText}" />

<!-- With inner content -->
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

## MCheckBox

```xml
<MCheckBox Content="Enable feature" IsChecked="{Binding IsEnabled}" />

<!-- Variants -->
<MCheckBox Variant="Default" Content="Default" />
<MCheckBox Variant="Card" Content="Card style" />
```

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

<!-- Variants -->
<MListBox Variant="Default" ... />
<MListBox Variant="Sidebar" ... />   <!-- navigation sidebar style -->
<MListBox Variant="NoStyle" ... />   <!-- unstyled container -->
```

## MIcon

```xml
<!-- Basic icon with size -->
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

<!-- Styles -->
<MIcon IconStyle="Default" IconSize="32" IconSource="..." />
<MIcon IconStyle="OnlyBorder" IconSize="32" IconSource="..." />
```

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

## MAlert

```xml
<MAlert Title="Heads up!" Message="Something important to know." />
<MAlert Variant="Destructive" Title="Error!" Message="Something went wrong." />
```

## MBadge

```xml
<MBadge Content="New">
    <MButton Content="Notifications" />
</MBadge>
```

## MAvatar

```xml
<MAvatar Source="{Binding AvatarUrl}" Initials="AB" Size="40" />
```

## MProgressRing

```xml
<MProgressRing IsIndeterminate="True" Width="32" Height="32" />
<MProgressRing Value="0.65" Width="48" Height="48" />
```

## MScrollViewer

```xml
<MScrollViewer MaxHeight="300">
    <MPanel Mode="Vertical" Spacing="8">
        <!-- Long content -->
    </MPanel>
</MScrollViewer>
```

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
        <!-- Toolbar buttons -->
    </MTabTitleBar.RightContent>
</MTabTitleBar>
```

## MViewHost — Region Navigation

```xml
<!-- Displays the view for the current ViewModel -->
<MViewHost ViewModel="{Binding RegionManager.SelectedItem}" />
```

## MSlider

```xml
<MSlider Minimum="0" Maximum="100" Value="{Binding Volume}" />
<MSlider Orientation="Vertical" Minimum="0" Maximum="1" Value="{Binding Level}" />
```

## MRadioButton

```xml
<MRadioButton Header="Option A" GroupName="MyGroup" IsChecked="{Binding IsA}" />
<MRadioButton Header="Option B" GroupName="MyGroup" />

<!-- Bordered variant (card-style) -->
<MRadioButton Variant="Bordered" Header="Premium" GroupName="plan" />
```

## MAutoCompleteBox

```xml
<MAutoCompleteBox ItemsSource="{Binding Suggestions}"
                  Text="{Binding SearchText}"
                  Watermark="Type to search..." />
```

## MProgressBar

```xml
<MProgressBar Value="65" Minimum="0" Maximum="100" />
<MProgressBar IsIndeterminate="True" />
<MProgressBar Variant="Destructive" Value="30" />

<!-- Custom fill color -->
<MProgressBar BarBrush="{MBrush Primary}" Value="50" />
```

## MHyperlinkButton

```xml
<MHyperlinkButton NavigateUri="https://example.com" Content="Visit site" />
<MHyperlinkButton Variant="Subtle" Content="Learn more" Command="{Binding OpenDocsCommand}" />
```

## MSeparator

```xml
<!-- Horizontal divider (default) -->
<MSeparator />

<!-- Vertical divider -->
<MSeparator Orientation="Vertical" />
```

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

## MItems

Generic items container that wraps each item in a styled `MItem` (ContentControl):

```xml
<MItems ItemsSource="{Binding Tags}">
    <MItems.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding}" />
        </DataTemplate>
    </MItems.ItemTemplate>
</MItems>
```

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

## MToggleSwitch

```xml
<MToggleSwitch IsChecked="{Binding IsEnabled}" />
<MToggleSwitch IsChecked="{Binding DarkMode}" OnContent="On" OffContent="Off" />
```

## MNumericUpDown

```xml
<MNumericUpDown Value="{Binding Count}" Minimum="0" Maximum="100" Increment="1" />
```

## MAutoCompleteBox

```xml
<MAutoCompleteBox ItemsSource="{Binding Suggestions}"
                  Text="{Binding SearchText}"
                  Watermark="Type to search..." />
```

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

## MBackground + MDialogTemplate — Dialog Wrapper

Use these together for all dialog views:

```xml
<MView x:Class="MyApp.Views.ConfirmView"
       xmlns="https://github.com/avaloniaui"
       xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
       xmlns:local="clr-namespace:MyApp.Views"
       x:DataType="local:ConfirmView_Model"
       Design.DataContext="{x:Static local:ConfirmView_Model.Design}">
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

## MCanvasGrid — Infinite Pan/Zoom Canvas

Interactive infinite canvas (zoom/pan via scroll wheel, pinch, right-click drag, Space+drag):

```xml
<MCanvasGrid Zoom="{Binding Zoom}"
             Offset="{Binding Offset}"
             ShowGrid="True"
             GridSize="20"
             MinZoom="0.1"
             MaxZoom="10">
    <!-- Content positioned absolutely -->
    <Canvas>
        <Border Canvas.Left="100" Canvas.Top="100" Width="120" Height="80" />
    </Canvas>
</MCanvasGrid>
```

Key bindings: `Ctrl+Scroll` = zoom · `Scroll` = pan · `Cmd+1` = reset · `Cmd+2` = fit all
