# Manta Internal Development Skills

Skills for developing the Manta UI toolkit itself.

## Available Skills

### [new-control.md](new-control.md)
Create new Manta controls. Covers:
- Control class with `M` prefix
- Theme file (AXAML) with `Design.PreviewWith`
- Registration in `MantaTheme.axaml`
- `SmoothBrushTransition` animations
- Moving base controls to Supported

## Quick Reference

### Adding a New Control

1. Create `/Manta/Controls/M{Name}.cs`
2. Create `/Manta/Controls/M{Name}.axaml` with `Design.PreviewWith`
3. Register in `/Manta/Themes/MantaTheme.axaml`
4. Move base control from `Unsupported/` to `Supported/` (if applicable)

### Key Conventions

- All controls use `M` prefix
- Variants use `M{ControlName}Variant` enum
- Use `SmoothBrushTransition` for color animations
- Include `Design.PreviewWith` for auto-demo generation
