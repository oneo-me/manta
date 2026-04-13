---
name: manta
description: >
  Manta UI toolkit for Avalonia 12 (.NET 10): MVVM framework, 60+ controls, theme system, i18n.
  Use for any Avalonia desktop project using Manta controls, ViewModels, styling, or navigation.
---

# Manta Toolkit — Quick Reference

Manta is an Avalonia 12 UI toolkit (.NET 10) providing 60+ themed controls, a reactive MVVM
framework, and a design system built on the OKLch color space.

## Technology Stack

| Component | Version |
|-----------|---------|
| .NET | 10.0 |
| Avalonia | 12.0.0 |
| DynamicData | 9.4.31 |
| YamlDotNet | 17.0.1 |
| C# | latest (nullable, implicit usings, compiled bindings) |

## Core Conventions

- All Manta types are in the `https://github.com/avaloniaui` XML namespace — **no extra xmlns
  prefix** needed in AXAML.
- Control names use the **`M` prefix**: `MButton`, `MCard`, `MPanel`, `MTextBox`, etc.
- **All views are `MView`** — including main windows and dialogs. The framework wraps views in
  native windows automatically. There is no separate Window AXAML class.
- ViewModel naming: `MyView_Model` paired with view `MyView`.
- Commands are computed properties returning `CommandWarper` (cached by property name).
- `SmoothBrushTransition` replaces Avalonia's `BrushTransition` for correct OKLch interpolation.
- Only use built-in `MBrushKey` color tokens — no custom color resources.
- Layout primitives (`Grid`, `Border`, `Canvas`, `StackPanel`, `TextBlock`) are always OK;
  for interactive controls always use Manta M\* equivalents.

## MVVM Minimal Example

```csharp
// ── ViewModel ──────────────────────────────────────
[View<MyView>]                                       // source generator binds View ↔ ViewModel
public sealed partial class MyView_Model : ViewModel
{
    public string Title { get; set => SetField(ref field, value); } = "Hello";

    public CommandWarper SaveCommand => GetCommand(() => { /* sync */ });
    public CommandWarper LoadCommand => GetCommand(async () => await LoadAsync());

    protected override void OnViewActivated(CompositeDisposable disposables)
    {
        MApp.SubscribeThemeChanged(_ => { }).DisposeWith(disposables);
    }
}
```

```xml
<!-- ── View (AXAML) ─────────────────────────────── -->
<MView x:Class="MyApp.Views.MyView"
       xmlns="https://github.com/avaloniaui"
       xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
       xmlns:local="clr-namespace:MyApp.Views"
       x:DataType="local:MyView_Model">
  <Design.DataContext>
    <local:MyView_Model />
  </Design.DataContext>
  <MPanel Mode="Vertical" Spacing="12" Margin="16">
    <TextBlock Text="{Binding Title}" FontSize="20" FontWeight="SemiBold" />
    <MButton Content="Save" Command="{Binding SaveCommand}" />
  </MPanel>
</MView>
```

```csharp
// ── Code-behind (only InitializeComponent) ─────────
public sealed partial class MyView : MView
{
    public MyView()
    {
        InitializeComponent();
    }
}
```

## Colors & Spacing — Cheat Sheet

```xml
<Border Background="{MBrush Background}"
        BorderBrush="{MBrush Border}"
        BorderThickness="{MThickness Px}"
        CornerRadius="{MRadius LG}"
        Padding="{MThickness S3}"
        BoxShadow="{MShadow MD}" />

<!-- Opacity / lightness variants -->
<Border Background="{MBrush Primary, Opacity=0.12}" />
<Border Background="{MBrush Primary, Type=Light2, Opacity=0.3}" />

<!-- Light / Dark conditional -->
<Setter Property="Opacity" Value="{OnTheme 1.0, Dark=0.85}" />
```

## Reference Files (load on demand)

These files contain detailed documentation. Load them when you need specifics:

- **[mvvm.md](references/mvvm.md)** — Full MVVM class hierarchy (ViewModel / WindowModel /
  DialogModel / RegionModel), command patterns (CommandWarper, IsExecuting,
  CanExecute), service registration, view lifecycle (OnViewActivated), ViewContext parent-child
  data sharing, `MViewHost` data passing with `IViewHostTarget<T>`, ViewTemplate,
  RegionManager tab navigation, MAlert / MConfirm quick dialogs, View source generator.
  **Load when:** implementing ViewModels, commands, services, navigation, or dialogs.

- **[controls.md](references/controls.md)** — AXAML usage examples for the current Manta control
  set: MPanel,
  MButton, MCard, MExpander, MAccordion, MTextBox, MCheckBox, MRadioButton, MToggleSwitch,
  MListBox, MIcon, MComboBox, MAlert, MBadge, MProgressRing, MProgressBar, MTabStrip,
  MTabControl, MViewHost, MSlider, MAutoCompleteBox, MNumericUpDown, MHyperlinkButton,
  MSeparator, MMenu, MContextMenu, MTree, MItems, MCanvasGrid, MDialogTemplate, MBackground,
  MImage, etc. Avalonia whitelist and forbidden control list. Variant enums.
  **Load when:** composing UIs, choosing controls, or looking up control properties.

- **[theme-colors.md](references/theme-colors.md)** — All MBrushKey tokens, MThickness / MRadius /
  MShadow / MSize scales with pixel values, SmoothBrushTransition usage, OnTheme conditional
  values, Filter syntax for selective sides/corners.
  **Load when:** styling controls, working with the theme system, or choosing spacing/radius.

- **[project-setup.md](references/project-setup.md)** — Complete project skeleton (.csproj,
  App.axaml, App.axaml.cs), MApp full API, MAppOptions, XmlnsDefinition namespace mapping,
  theme switching, Generators.props build integration, ViewGenerator, Logging system (Log /
  ConsoleLogger / FileLogger), Assets resource loading, UIThread dispatcher, single-instance
  enforcement, Service registration pattern.
  **Load when:** starting a new project, wiring up MApp, or using framework services.

- **[platform.md](references/platform.md)** — Platform dialog APIs (OpenFilePickerAsync,
  OpenFolderPickerAsync, SaveFilePickerAsync, GetClipboardAsync), CurrentSystem platform
  detection, WindowStatus observable, VisualExtensions (BindTitleBarEvents), WindowExtensions
  (Centered, FocusActivate), ImageLoadService async image cache (Web/Local/Avalonia sources).
  **Load when:** accessing platform APIs, loading images, or handling window behavior.

- **[i18n.md](references/i18n.md)** — Full globalization guide: YAML file structure, @config.yml
  options, LangGenerator source generator (LangKey / LangData output, diagnostic codes
  LANG001/LANG002), Lang.Register(), Lang.Current, {Lang} / {LangEmoji} / {LangName} markup
  extensions, ViewModel usage, language selector UI pattern, LangType enum (45+ languages),
  multi-library registration with namespace isolation.
  **Load when:** adding multilingual support or switching languages at runtime.

- **[ui-design.md](references/ui-design.md)** — UI design guide for modern minimal layouts:
  typography scale, spacing rhythm (4px base), shadow depth, semantic color usage, icon patterns,
  button hierarchy, and complete AXAML examples for sidebar navigation, dashboard cards, settings
  pages, forms with labeled fields, grouped-list rows, and empty states.
  **Load when:** designing layouts or making interfaces look clean and professional.
