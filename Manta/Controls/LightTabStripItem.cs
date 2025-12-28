using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Manta.Controls;

public class LightTabStripItem : ListBoxItem
{
    public static readonly StyledProperty<LightTabStripType> TypeProperty = AvaloniaProperty.Register<LightTabStripItem, LightTabStripType>(
        nameof(Type));

    public LightTabStripType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<LightTabStrip>() is { } tabStrip)
            Type = tabStrip.Type;
    }
}
