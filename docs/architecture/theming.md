# Theming

Manta themes controls with dynamic resources rather than per-control literals. Color values are declared in the OKLch color space, and markup extensions resolve them for the active light or dark variant.

## Theme resources

| File | Contents |
| --- | --- |
| `Manta/Themes/@Light.axaml`, `@Dark.axaml` | `OklchColor` values keyed by `MBrushKey`, plus sizes and radii. |
| `Manta/Themes/@Base.axaml`, `@Shadows.axaml`, `@Rings.axaml` | Shared brushes, shadows and rings. |
| `Manta/Themes/MantaTheme.axaml` | Generated dictionary that merges everything and maps `Light`, `Dark` and `Default`. |
| `Manta/Controls/Design/Window.axaml` | Window chrome styles. |

Include `<MantaTheme />` in `MApp.Styles`. The theme dictionaries under the `Light` and `Dark` keys let Avalonia swap values when the requested variant changes.

## Theme variables

Theme markup extensions read resources and compute final values at binding time:

- `MBrush` resolves a `MBrushKey` brush, with `Type` and `Opacity` adjustments.
- `MSize`, `MRadius`, `MRing` and `MThickness` resolve spacing and shape tokens.
- `MShadow` resolves elevation shadows.
- `OnThemeExtension` chooses a value per theme variant and falls back to `Default`.

Prefer these over `{DynamicResource}` on raw keys, because they apply the variant-aware conversion consistently. Do not hardcode colors, radii or spacing in control XAML.

## Color pipeline

`OklchColor` converts to `RgbColor`, then to an Avalonia `Color`. Keeping the source values in OKLch makes lightness and chroma changes perceptually consistent across the light and dark palettes.

`SmoothBrushTransition` interpolates brushes for animated property changes. It handles solid and gradient brushes, mixes with a representative color when only one side matches, and interpolates premultiplied alpha so fading stays correct.

## Control themes

- Controls under `Manta/Controls` are authored, and each carries a `Design.PreviewWith` block that previews it in the designer.
- `Manta/Controls/Base` holds theme styles for Avalonia base controls: `Supported` for controls Manta styles, `Unsupported` for placeholders that direct users to the `M` control.
- `Manta/Controls/Design` holds window-level styles.

`MantaPrepare` generates the `Supported` and `Unsupported` trees by reflecting over the Avalonia control assembly and comparing it with the existing `M*` controls. It also regenerates `MantaTheme.axaml` and the demo screens described in [MantaDemo](#mantademo).

## Regeneration

Run `dotnet run --project MantaPrepare` after adding a control, editing a `Design.PreviewWith` block, or changing theme resources. The tool rewrites `MantaTheme.axaml`, the `Supported` and `Unsupported` control themes, and the demo files. Treat all of those as generated output and never edit them by hand.

## MantaDemo

`MantaDemo/Demos/AutoDemos` is generated per control. `DemoService` combines those generated demos with the hand-written demos under `Demos/StyleDemos`, `Demos/ReactiveDemos`, `Demos/MultiLanguageDemos` and `Demos/ControlDemos`, then groups them by `DemoGroup`. Add a showcase by writing its `Design.PreviewWith` block in the control, not by editing the auto-demos.
