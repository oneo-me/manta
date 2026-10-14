# Manta samples

Standalone applications that consume the published [ONEO.Manta](https://www.nuget.org/packages/ONEO.Manta) packages. They live outside `source/` and never reference the library projects, so they build the same way a real consumer would.

## MantaSample

A small desktop application that shows the most common Manta features:

- Startup through `MApp` with the `<MantaTheme />` style and the light/dark variants.
- MVVM with `[View<T>]`, `WindowModel`, `RegionModel` and `RegionManager<T>`.
- Commands through `CommandWarper`, plus the `MAlert` and `MConfirm` dialog helpers.
- Type-safe localization generated from `Langs/*.yml`, with a runtime language switch.
- Common controls such as `MTextBox`, `MCheckBox`, `MComboBox`, `MSlider` and `MProgressBar`.

## MantaComponents

A plain Avalonia application that adopts Manta without its framework. It keeps the stock Avalonia `Application`, `Window` and MVVM classes, and only adds:

- `<themes:MantaTheme />` to `Application.Styles`, so Manta styles both its own controls and the Avalonia base controls.
- Manta controls in the view, such as `MButton`, `MTextBox`, `MComboBox`, `MNumericUpDown`, `MCheckBox`, `MToggleSwitch`, `MSlider`, `MProgressBar`, `MProgressRing`, `MListBox`, `MBadge`, `MExpander` and `MSeparator`.
- Manta theme variables in its own styles, such as `{MBrush Card}` and `{MRadius LG}`.

View models are a hand-written `ObservableObject` and `RelayCommand`; the sample never uses `MApp`, `MView`, `Model`/`ViewModel`, regions, dialogs or the language generator. This is the path to pick when Manta should be a component library inside an existing Avalonia application rather than its application framework.

Build and run either sample from this folder:

```sh
dotnet run --project MantaSample
dotnet run --project MantaComponents
```

The samples pin the released package version in their `.csproj` files; bump it when a newer release is published. In `MantaSample` the `ONEO.Manta.Generators` reference is build-time only (`PrivateAssets="all"`), so the generator never reaches the published app.
