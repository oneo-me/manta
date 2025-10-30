using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace LightKit.Controls;

public class LightTabStripItem : ListBoxItem
{
    public static readonly StyledProperty<LightTabStripStyle> CustomProperty = AvaloniaProperty.Register<LightTabStripItem, LightTabStripStyle>(
        nameof(Custom));

    public LightTabStripStyle Custom
    {
        get => GetValue(CustomProperty);
        set => SetValue(CustomProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<LightTabStrip>() is { } tabStrip)
            Custom = tabStrip.Custom;
    }
}
