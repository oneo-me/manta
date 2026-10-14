# Changelog

## [Unreleased]

- Take the GitHub release notes from the matching `CHANGELOG.md` section instead of the generated commit list.
- Add `samples/MantaSample`, a standalone application that installs the published `ONEO.Manta` packages and shows the main features.
- Add `samples/MantaComponents`, a plain Avalonia application that uses only the `ONEO.Manta` theme and controls.
- Add `samples/MantaThemeSample`, a plain Avalonia application that uses only the `ONEO.Manta` theme variables.
- Add `samples/AvaloniaI18nSample`, a stock Avalonia application that uses only the `ONEO.Manta` localization feature, and rename the samples solution to `Samples.slnx`.
- Add a "Use only what you need" section to the website that shows how to adopt localization, theming, MVVM or the controls on their own.
- Highlight Native AOT compatibility on the website and in the documentation.
- Stop the website from overflowing horizontally and jittering on small screens when a code sample, tab or link is wider than the viewport.

## [1.2026.1] - 2026-10-14

- Release Manta for .NET 10 and Avalonia 12.
- Add the `M`-prefixed control set and themed styles for supported Avalonia controls.
- Add light and dark themes built on the OKLch color space.
- Add `Model`, `ViewModel`, `WindowModel` and `DialogModel` for the MVVM layer.
- Add the `[View<T>]` attribute, which generates view creation at compile time.
- Add region and tab navigation with `RegionManager<T>`, and modal dialogs with `DialogModel`.
- Add type-safe language keys generated from YAML, with runtime language switching.
- Ship the `MantaDemo` reference application.
