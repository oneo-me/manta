using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Manta.Controls;

public class MAutoCompleteBox : AutoCompleteBox
{
    TextBox? PART_TextBox;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        // Synchronize focus style with TextBox

        PART_TextBox = e.NameScope.Get<TextBox>(nameof(PART_TextBox));
        PART_TextBox.GotFocus += TextBoxOnGotFocus;
        PART_TextBox.LostFocus += TextBoxOnLostFocus;
    }

    void TextBoxOnGotFocus(object? sender, FocusChangedEventArgs focusChangedEventArgs)
    {
        PseudoClasses.Set(":focus", true);
    }

    void TextBoxOnLostFocus(object? sender, RoutedEventArgs routedEventArgs)
    {
        PseudoClasses.Set(":focus", false);
    }
}
