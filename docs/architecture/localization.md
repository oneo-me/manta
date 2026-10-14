# Localization

Manta stores translations in YAML files and compiles them into the assembly. A Roslyn generator produces type-safe keys and the lookup table, so views bind to language keys instead of literals.

## Language files

Language files live in a `Langs` folder next to the project and are named after a `LangType` value, for example `en.yml` or `zh_Hans.yml`.

- The base file, `en.yml` by default, defines the full key set.
- Every other file overrides the base file's keys.
- An optional `@config.yml` sets `base`, `comment`, `modifier` and `classPrefix`. The `comment` file supplies XML documentation for generated keys; the `modifier` and `classPrefix` control the generated class names.

Keys map directly to values:

```yaml
Dialog_Title: Information
Dialog_Cancel: Cancel
```

## Generation

`LangGenerator` consumes every `Langs/*.yml` file as an additional file. It emits two classes in the project's namespace:

- `LangKey` holds a `const string` per base key, prefixed with the assembly name so keys stay unique across assemblies.
- `LangData.Load()` returns a `LangDataBase` that resolves a key for a `LangType`.

The generator reports `LANG001` on parse failures and `LANG002` when a translated file is missing a base key or adds a redundant one. Fix those diagnostics rather than ignoring them.

## Runtime lookup

Register the generated data during startup with `Lang.Register(LangData.Load())`. `Lang.Current` selects the active `LangType`; the default comes from the system culture. The call works in any Avalonia `Application`; `MApp` is not required.

Resolution walks registered `LangDataBase` instances in registration order, then falls back to the base culture, then to the key itself. `Lang.Get` formats arguments against the current culture.

Views bind through the `{Lang}` markup extension, for example `Text="{Lang Dialog_Cancel}"` or `Text="{Lang {Binding Header}}"`. `Lang.GetBinding` returns the same binding from code. All `{Lang}` bindings observe `Lang.Current`, so they refresh when the language changes.

## Adding a language

1. Confirm the culture exists in `LangType`; the enum is the fixed set of supported cultures.
2. Add `Langs/<LangType>.yml` with the translated keys.
3. Build the project so the generator validates the file and regenerates `LangData`.

Translate keys only where the text differs from the base file; missing keys fall back to the base culture.
