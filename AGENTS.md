# Manta

Manta is an Avalonia UI toolkit and MVVM framework. Read [ARCHITECTURE.md](ARCHITECTURE.md) before coding, then the relevant subject under [Subjects](ARCHITECTURE.md#subjects).

## Principles

- Keep the library free of demo concerns; `Manta` never references `MantaDemo`.
- Never hand-edit generated files: `Manta/Themes/MantaTheme.axaml`, `Manta/Controls/Base/Supported`, `Manta/Controls/Base/Unsupported` and `MantaDemo/Demos/AutoDemos`. Run `dotnet run --project MantaPrepare` instead.
- New controls use the `M` prefix, live in `Manta/Controls`, and expose a `Design.PreviewWith` block so `MantaPrepare` can generate their demo screen.
- Keep behavior in C# and styling in XAML. Resolve colors and spacing through theme variables such as `MBrush`; do not hardcode them.
- Route user-facing text through `LangKey` and the `{Lang}` markup; never hardcode it in views.
- Treat warnings as errors and keep the public API AOT-compatible, as the build settings already require.

## Working with the user

- Confirm before changing the public API, the theme variable set or the demo structure.
- Do not commit, tag or publish unless the user asks.
- Keep commits focused and use Conventional Commits.

## Verification

Build and run the demo as described in [README.md](README.md#development). There is no test project, so exercise the affected screen in `MantaDemo` and watch the source generator diagnostics. Regenerate derived files after changing control AXAML or theme resources.

## Documentation

- Update a document only when a fact its reader needs changes.
- Keep each document within its budget and written for its reader.
- Record completed user-visible outcomes as short `Unreleased` bullets in [CHANGELOG.md](CHANGELOG.md), merging related ones.
- Repair documents you touch instead of only appending to them.
- Keep `README.md` and `README.zh-CN.md` equivalent; everything else, including code comments, is English.
- Keep documentation on the code branch and reconcile it during merges.
