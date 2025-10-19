using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace LightKit.Controls;

public class LightListBoxItem : ListBoxItem
{
    protected override Type StyleKeyOverride { get; } = typeof(LightListBoxItem);

    public static readonly StyledProperty<LightListBoxStyle> CustomProperty = AvaloniaProperty.Register<LightListBoxItem, LightListBoxStyle>(
        nameof(Custom));

    public LightListBoxStyle Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<LightListBox>() is { } listBox)
            Custom = listBox.Custom;
    }
}
