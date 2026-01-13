# Manta Skills

This directory contains skill documents for working with the Manta UI toolkit.

## Skill Categories

### [internal/](internal/) - Manta Development

Skills for developing the Manta library itself:

| Skill | Description |
|-------|-------------|
| [new-control.md](internal/new-control.md) | Create new Manta controls |

### [user/](user/) - Using Manta

Skills for projects using Manta as a dependency:

| Skill | Description |
|-------|-------------|
| [project-structure.md](user/project-structure.md) | Project setup and organization |
| [views.md](user/views.md) | MVVM architecture (ViewModel, WindowModel, RegionModel) |
| [theme-resources.md](user/theme-resources.md) | Theme markup extensions (MBrush, MSize, etc.) |
| [controls.md](user/controls.md) | Control library reference |
| [services.md](user/services.md) | Service locator and dependency injection |

## Quick Reference

### For Manta Developers

```
Creating a control:
1. /Manta/Controls/M{Name}.cs
2. /Manta/Controls/M{Name}.axaml (with Design.PreviewWith)
3. Register in MantaTheme.axaml
```

### For Manta Users

```xml
<!-- Add theme -->
<Application.Styles>
    <MantaTheme />
</Application.Styles>

<!-- Use controls -->
<MButton Content="Click me" Variant="Primary" />
<MPanel Mode="Vertical" Spacing="{MSize S3}">
    <TextBlock Text="Hello" />
</MPanel>
```

## Key Concepts

| Concept | Internal | User |
|---------|----------|------|
| Control Creation | new-control.md | - |
| Project Structure | - | project-structure.md |
| Views/ViewModels | - | views.md |
| Theme Resources | - | theme-resources.md |
| Control Usage | - | controls.md |
| Services | - | services.md |
