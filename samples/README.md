# Manta samples

Standalone applications that consume the published [ONEO.Manta](https://www.nuget.org/packages/ONEO.Manta) packages. They live outside `source/` and never reference the library projects, so they build the same way a real consumer would.

## MantaSample

A small desktop application that shows the most common Manta features:

- Startup through `MApp` with the `<MantaTheme />` style and the light/dark variants.
- MVVM with `[View<T>]`, `WindowModel`, `RegionModel` and `RegionManager<T>`.
- Commands through `CommandWarper`, plus the `MAlert` and `MConfirm` dialog helpers.
- Type-safe localization generated from `Langs/*.yml`, with a runtime language switch.
- Common controls such as `MTextBox`, `MCheckBox`, `MComboBox`, `MSlider` and `MProgressBar`.

Build and run it from this folder:

```sh
dotnet run --project MantaSample
```

The sample pins the released package version in `MantaSample/MantaSample.csproj`; bump it when a newer release is published. The `ONEO.Manta.Generators` reference is build-time only (`PrivateAssets="all"`), so the generator never reaches the published app.
