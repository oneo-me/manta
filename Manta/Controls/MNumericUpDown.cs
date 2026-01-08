using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Manta.Controls;

public enum MNumericUpDownVariant
{
    Default
}

public class MNumericUpDown : NumericUpDown
{
    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<MNumericUpDown, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }

    public static readonly StyledProperty<MNumericUpDownVariant> VariantProperty = AvaloniaProperty.Register<MNumericUpDown, MNumericUpDownVariant>(
        nameof(Variant));

    public MNumericUpDownVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    TextBox? PART_TextBox;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        // Synchronize focus style with TextBox

        PART_TextBox = e.NameScope.Get<TextBox>("PART_TextBox");
        PART_TextBox.GotFocus += TextBoxOnGotFocus;
        PART_TextBox.LostFocus += TextBoxOnLostFocus;
    }

    void TextBoxOnGotFocus(object? sender, GotFocusEventArgs gotFocusEventArgs)
    {
        PseudoClasses.Set(":focus", true);
    }

    void TextBoxOnLostFocus(object? sender, RoutedEventArgs routedEventArgs)
    {
        PseudoClasses.Set(":focus", false);
    }
}
