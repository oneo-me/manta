using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Manta.Controls;

public class LightListBoxItem : ListBoxItem
{
    public static readonly StyledProperty<LightListBoxType> TypeProperty = AvaloniaProperty.Register<LightListBoxItem, LightListBoxType>(
        nameof(Type));

    public LightListBoxType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (this.FindAncestorOfType<LightListBox>() is { } listBox)
            Type = listBox.Type;
    }
}
