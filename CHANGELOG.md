# Changelog

## [Unreleased]

- Take the GitHub release notes from the matching `CHANGELOG.md` section instead of the generated commit list.
- Add `samples/MantaSample`, a standalone application that installs the published `ONEO.Manta` packages and shows the main features.
- Add `samples/MantaComponents`, a plain Avalonia application that uses only the `ONEO.Manta` theme and controls.
- Add `samples/AvaloniaI18nSample`, a stock Avalonia application that uses only the `ONEO.Manta` localization feature, and rename the samples solution to `Samples.slnx`.

## [1.2026.1] - 2026-10-14

- Release Manta for .NET 10 and Avalonia 12.
- Add the `M`-prefixed control set and themed styles for supported Avalonia controls.
- Add light and dark themes built on the OKLch color space.
- Add `Model`, `ViewModel`, `WindowModel` and `DialogModel` for the MVVM layer.
- Add the `[View<T>]` attribute, which generates view creation at compile time.
- Add region and tab navigation with `RegionManager<T>`, and modal dialogs with `DialogModel`.
- Add type-safe language keys generated from YAML, with runtime language switching.
- Ship the `MantaDemo` reference application.
