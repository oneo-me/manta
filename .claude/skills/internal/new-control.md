# Skill: Create New Manta Control

Use this skill when adding a new control to the Manta UI toolkit.

## Prerequisites

- Identify the base Avalonia control to extend (if any)
- Decide on the control name (must start with `M` prefix)
- Determine required variants/properties

## Steps

### 1. Create Control Class

Create `/Manta/Controls/M{ControlName}.cs`:

```csharp
using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

// Optional: Define variants if the control has multiple styles
public enum M{ControlName}Variant
{
    Default,
    // Add other variants as needed
}

public class M{ControlName} : {BaseControl}  // e.g., ContentControl, Control, Button
{
    // Optional: Variant property
    public static readonly StyledProperty<M{ControlName}Variant> VariantProperty =
        AvaloniaProperty.Register<M{ControlName}, M{ControlName}Variant>(nameof(Variant));

    public M{ControlName}Variant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    // Add additional styled properties as needed
    public static readonly StyledProperty<object?> CustomProperty =
        AvaloniaProperty.Register<M{ControlName}, object?>(nameof(Custom));

    public object? Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
    }
}
```

### 2. Create Theme File

Create `/Manta/Controls/M{ControlName}.axaml`:

```xml
<ResourceDictionary
    xmlns="https://github.com/avaloniaui"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    x:ClassModifier="internal">

    <!-- IMPORTANT: Design.PreviewWith section for auto-demo -->
    <Design.PreviewWith>
        <MPanel Mode="Vertical" Spacing="12" Margin="12">
            <M{ControlName} />
            <M{ControlName} Variant="..." />
            <!-- Add more preview examples -->
        </MPanel>
    </Design.PreviewWith>

    <ControlTheme x:Key="{x:Type M{ControlName}}" TargetType="M{ControlName}">
        <Setter Property="Template">
            <ControlTemplate>
                <Border Background="{TemplateBinding Background}"
                        BorderBrush="{TemplateBinding BorderBrush}"
                        CornerRadius="{TemplateBinding CornerRadius}"
                        BorderThickness="{TemplateBinding BorderThickness}"
                        Padding="{TemplateBinding Padding}">
                    <!-- Control content here -->
                    <ContentPresenter Name="PART_ContentPresenter"
                                      Content="{TemplateBinding Content}"
                                      ContentTemplate="{TemplateBinding ContentTemplate}" />
                </Border>
            </ControlTemplate>
        </Setter>

        <!-- Default property values -->
        <Setter Property="Background" Value="{MBrush Background}" />
        <Setter Property="BorderBrush" Value="{MBrush Border}" />
        <Setter Property="CornerRadius" Value="{MRadius MD}" />

        <!-- Add SmoothBrushTransition for color animations -->
        <Setter Property="Transitions">
            <Transitions>
                <SmoothBrushTransition Property="Background" Duration="0:0:0.1" />
                <SmoothBrushTransition Property="BorderBrush" Duration="0:0:0.1" />
            </Transitions>
        </Setter>

        <!-- State styles -->
        <Style Selector="^:pointerover">
            <Setter Property="Background" Value="{MBrush Foreground, Opacity=0.05}" />
        </Style>

        <Style Selector="^:disabled">
            <Setter Property="Opacity" Value="0.5" />
        </Style>

        <!-- Variant styles -->
        <Style Selector="^[Variant=Default]">
            <!-- Default variant styles -->
        </Style>
    </ControlTheme>
</ResourceDictionary>
```

### 3. Register in MantaTheme.axaml

Add to `/Manta/Themes/MantaTheme.axaml` in the Controls section:

```xml
<MergeResourceInclude Source="../Controls/M{ControlName}.axaml" />
```

### 4. Move Base Control to Supported (if applicable)

If wrapping an existing Avalonia control:

1. Move `/Manta/Controls/Base/Unsupported/{BaseControl}.axaml`
   to `/Manta/Controls/Base/Supported/{BaseControl}.axaml`

2. Update the content to show usage guidance:

```xml
<ResourceDictionary
    xmlns="https://github.com/avaloniaui"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    x:ClassModifier="internal">
    <Design.PreviewWith>
        <TextBlock Text="Please use the 'M{ControlName}'" />
    </Design.PreviewWith>
    <ControlTheme x:Key="{x:Type {BaseControl}}" TargetType="{BaseControl}">
        <Setter Property="Template">
            <ControlTemplate>
                <TextBlock Text="Please use the 'M{ControlName}'" Foreground="Red" />
            </ControlTemplate>
        </Setter>
    </ControlTheme>
</ResourceDictionary>
```

3. Update the reference in `MantaTheme.axaml` from Unsupported to Supported

## Key Patterns

### Animation Transitions

Always use `SmoothBrushTransition` for brush/color animations:

```xml
<Setter Property="Transitions">
    <Transitions>
        <SmoothBrushTransition Property="Background" Duration="0:0:0.1" />
    </Transitions>
</Setter>
```

### Common Template Parts

- `PART_ContentPresenter` - Main content
- `PART_HeaderPresenter` - Header content
- Named elements like `Border#Icon` for targeting in styles

## Checklist

- [ ] Control class created with `M` prefix
- [ ] Variants defined (if applicable)
- [ ] Theme file created with `Design.PreviewWith` section
- [ ] Registered in `MantaTheme.axaml`
- [ ] Base control moved to Supported (if applicable)
- [ ] `SmoothBrushTransition` added for color properties
