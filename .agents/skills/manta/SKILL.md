---
name: manta
description: >
  Expert guide for building Avalonia desktop applications using the Manta UI toolkit.
  Covers MVVM patterns (ViewModel/WindowModel/DialogModel/TrayWindowModel/@ViewAttribute/commands/services),
  region navigation (RegionManager/RegionModel), single-slot navigation (ViewManager),
  theme & color system (MBrush/MThickness/MRadius/MShadow/MSize/SmoothBrushTransition/OnTheme),
  globalization/i18n (Lang/LangKey/LangType), platform APIs (file/folder pickers, clipboard),
  and 60+ built-in controls
  (MPanel/MButton/MCard/MTextBox/MIcon/MListBox/MSlider/MTree/MMenu/MCanvas/MRadioButton/MToggleSwitch
  /MAutoCompleteBox/MNumericUpDown/MAccordion/MCommandPanel etc.).

  Use when the user is:
  - Setting up a new Manta-based Avalonia project
  - Implementing ViewModels, commands, or service registration
  - Choosing between WindowModel, DialogModel, TrayWindowModel, ViewModel, or RegionModel
  - Setting up tab/region navigation with RegionManager or single-slot switching with ViewManager
  - Opening modal dialogs and reading results
  - Styling controls with theme colors (MBrush), spacing (MThickness), or corner radius (MRadius)
  - Using or composing Manta controls in AXAML
  - Asking which Avalonia native controls are allowed vs must use Manta equivalents
  - Asking "how do I use MBrush / MThickness / MRadius / MShadow / MSize"
  - Asking how to create commands, bind views, or wire up the app entry point
  - Opening file/folder pickers or save dialogs from a ViewModel
  - Adding multilingual/localization support (Lang, LangKey, LangType, YAML files)
  - Using {Lang}, {LangEmoji}, {LangName} markup extensions in AXAML
  - Switching languages at runtime or building a language selector
  - Designing UI layouts, choosing spacing/typography, or asking how to build a clean modern interface
  - Asking how to implement sidebar navigation, settings pages, forms, dashboards, empty states
  - Asking about shadow depth, color semantics, icon patterns, or button hierarchy
---

# Manta Toolkit

Manta is an Avalonia 11 UI toolkit (.NET 10) providing a themed control library (60+ controls),
a reactive MVVM framework, and a design system built on the OKLch color space.

## Key Conventions

- All Manta controls/types are in the global `https://github.com/avaloniaui` namespace — no extra
  `xmlns` prefix needed in AXAML.
- Control names use the `M` prefix: `MButton`, `MCard`, `MPanel`, `MTextBox`, etc.
- **All views are `MView`** — including main window views and dialog views. There is no separate
  window AXAML class.
- ViewModel naming convention: `MyView_Model` paired with view `MyView`.
- Commands are defined as computed properties returning `CommandWarper` (cached by property name).
- `SmoothBrushTransition` replaces Avalonia's `BrushTransition` for correct OKLch interpolation.
- Only use built-in `MBrushKey` color tokens — no custom color resources.
- Layout primitives (`Grid`, `Border`, `Canvas`, `TextBlock`) are always OK; prefer Manta M\*
  equivalents for interactive controls (see controls.md for the full allowed/forbidden list).

## Quick Reference

### MVVM Basics

All logic lives in ViewModels. Code-behind only calls `InitializeComponent()`.

```csharp
[View<MyView>]
public sealed class MyView_Model : ViewModel
{
    public static MyView_Model Design => new();  // design-time data

    // Reactive property
    public string Title { get; set => SetValue(ref field, value); } = "Hello";

    // Command
    public CommandWarper SaveCommand => GetCommand(() => { /* ... */ });

    // Async command
    public CommandWarper LoadCommand => GetCommand(async () => await LoadDataAsync());

    // View lifecycle — subscriptions auto-cleaned when view detaches
    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        MApp.SubscribeThemeChanged(_ => UpdateIcon()).DisposeWith(disposables);
    }
}
```

### Colors & Spacing

```xml
<Border Background="{MBrush Background}"
        BorderBrush="{MBrush Border}"
        BorderThickness="{MThickness Px}"
        CornerRadius="{MRadius LG}"
        Padding="{MThickness S3}" />

<!-- With opacity / lightness variants -->
<Border Background="{MBrush Primary, Opacity=0.12}" />
<Border Background="{MBrush Primary, Type=Light2, Opacity=0.3}" />
```

### Layout & Controls

```xml
<MPanel Mode="Vertical" Spacing="12" Margin="16">
    <TextBlock Text="Title" FontSize="20" FontWeight="SemiBold" />
    <MTextBox Placeholder="Enter name..." Text="{Binding Name}" />
    <MButton Content="Save" Command="{Binding SaveCommand}" />
</MPanel>
```

## Reference Files

Load these when you need detailed information:

- **[mvvm.md](references/mvvm.md)** — Full MVVM class hierarchy (ViewModel/WindowModel/DialogModel/
  TrayWindowModel/RegionModel), command patterns, service registration, OnViewActivated lifecycle,
  ViewContext, ViewManager, ViewTemplate, RegionManager, App wiring. Load when implementing
  ViewModels, commands, services, navigation, or dialogs.

- **[theme-colors.md](references/theme-colors.md)** — All MBrushKey tokens, MThickness/MRadius/
  MShadow/MSize scales with values, SmoothBrushTransition usage, OnTheme conditional values.
  Load when styling controls or working with the theme system.

- **[controls.md](references/controls.md)** — AXAML usage examples for all major controls:
  MPanel, MButton, MCard, MExpander, MAccordion, MTextBox, MCheckBox, MRadioButton, MToggleSwitch,
  MListBox, MIcon, MComboBox, MAlert, MBadge, MAvatar, MProgressRing, MProgressBar, MTabStrip,
  MTabControl, MViewHost, MSlider, MAutoCompleteBox, MNumericUpDown, MHyperlinkButton, MSeparator,
  MMenu, MContextMenu, MTree, MItems, MCanvasGrid, MDialogTemplate, MBackground.
  Load when composing UIs.

- **[project-setup.md](references/project-setup.md)** — Complete project skeleton: .csproj,
  App.axaml, App.axaml.cs, MainWindow files, MAppOptions config, multi-window, theme switching.
  Load when starting a new project or wiring up the application entry point.

- **[platform.md](references/platform.md)** — Platform dialog APIs: `OpenFilePickerAsync`,
  `OpenFolderPickerAsync`, `SaveFilePickerAsync`, `GetClipboardAsync` — ViewModel extension
  methods that wrap Avalonia's StorageProvider with automatic thread safety.

- **[i18n.md](references/i18n.md)** — Full globalization/localization guide: YAML file structure,
  source generator output (LangKey), `Lang.Register()`, `Lang.Current`, `{Lang}` / `{LangEmoji}`
  / `{LangName}` markup extensions, ViewModel usage, language selector UI pattern, LangType enum.
  Load when adding multilingual support or switching languages at runtime.

- **[ui-design.md](references/ui-design.md)** — UI design guide for modern minimal layouts:
  typography scale, spacing rhythm, shadow depth, semantic color usage, icon patterns, button
  hierarchy, and complete AXAML examples for sidebar nav, dashboard cards, settings pages, forms
  with labeled fields, grouped-list rows, and empty states. Load when designing layouts or asking
  how to make interfaces look clean and professional.
